﻿using DrawShip.Common.ComInterop;
using System;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
    using System.Windows.Forms;

    /// <summary>
	/// The context of the process/application
	/// </summary>
	public class ApplicationContext
    {
        private enum _RunMode
        {
            None,
            Run,
            Install,
            Uninstall
        }

        private readonly _RunMode _mode = _RunMode.Run;

        public ApplicationContext()
        {
            var commandLine = Environment.GetCommandLineArgs().Skip(1).ToArray();
            FileName = commandLine.FirstOrDefault(a => !a.StartsWith("/"));
            if (FileName != null)
            {
                FileName = Path.GetFileNameWithoutExtension(FileName);
                WorkingDirectory = Path.GetDirectoryName(FileName);
            }

            var configuredDir = commandLine.SingleOrDefault(arg => arg.StartsWith("/dir:"));
            if (configuredDir != null)
                WorkingDirectory = configuredDir.Substring("/dir:".Length);

            if (string.IsNullOrEmpty(WorkingDirectory))
                WorkingDirectory = Environment.CurrentDirectory;

            var formatString = commandLine.SingleOrDefault(arg => arg.StartsWith("/format:"));
            if (formatString != null)
            {
                DiagramFormat format;
                if (Enum.TryParse(formatString.Substring("/format:".Length), true, out format))
                    Format = format;
            }

            var mode = commandLine.SingleOrDefault(arg => arg.StartsWith("/mode:"));
            if (mode != null)
                Enum.TryParse(mode.Substring("/mode:".Length), true, out _mode);

            FileSystemFactory = new FileSystemFactory();
            RendererFactory = new RendererFactory();

            if (_mode == _RunMode.Run)
                _mode = ModifyModeIfRunFromInstallationDirectory();
        }

        private static _RunMode ModifyModeIfRunFromInstallationDirectory()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length != 1 || Environment.CommandLine.Trim() != $@"""{args.Single()}""")
                return _RunMode.Run;

            var isInstalled = InstallRunMode.IsInstalled();

            //no arguments supplied; if we're also running from the installation directory, maybe we need to install/uninstall
            if (isInstalled && MessageBox.Show("Do you wish to uninstall DrawShip?", "DrawShip", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                return _RunMode.Uninstall;
            if (!isInstalled && MessageBox.Show("Do you wish to install DrawShip?", "DrawShip", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return _RunMode.Install;

            return _RunMode.None;
            ;
        }

        /// <summary>
        /// Get the run mode for the process/application
        /// </summary>
        /// <returns></returns>
        public IRunMode GetRunMode()
        {
            switch (_mode)
            {
                case _RunMode.None:
                    return new NullRunMode();

                case _RunMode.Install:
                    return new InstallRunMode();

                case _RunMode.Uninstall:
                    return new UninstallRunMode();

                default:
                    return new SelfHostRunMode(
                        new OpenDrawingInOtherHostRunMode());
            }
        }

        /// <summary>
        /// The file-system factory to use to get a file-system instance
        /// </summary>
        public FileSystemFactory FileSystemFactory { get; }

        /// <summary>
        /// The factory to use to get a drawing renderer instance
        /// </summary>
        public RendererFactory RendererFactory { get; }

        /// <summary>
        /// The filename given to this process/application
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The working directory of this process/application
        /// </summary>
        public string WorkingDirectory { get; }

        /// <summary>
        /// The format to be used for rendering the diagram (e.g. Html/Image)
        /// </summary>
        public DiagramFormat Format { get; set; }

        /// <summary>
        /// Print the drawing requested for this process/application, without asking another process to
        /// </summary>
        /// <param name="command"></param>
        public void PrintDrawing(ShowDiagramStructure command)
        {
            var tempFile = _CreateTempFile();

            var renderer = RendererFactory.GetImageRenderer();
            var fileSystem = FileSystemFactory.GetFileSystem(null);
            var drawing = command.GetDrawing();
            var viewModel = new DrawingViewModel(drawing, fileSystem, null, null)
            {
                PageIndex = command.PageIndex
            };

            using (var writeStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var result = renderer.RenderDrawing(viewModel);
                result.WriteResult(writeStream);
            }

            Wia.Print(null, tempFile);
        }

        private static string _CreateTempFile()
        {
            try
            {
                return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
            }
            catch (Exception exc)
            {
                throw new IOException("Could not create temporary file for printing - " + exc.Message);
            }
        }
    }
}
