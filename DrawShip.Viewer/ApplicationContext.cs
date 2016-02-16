using DrawShip.ComInterop;
using DrawShip.Common;
using System;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
	public class ApplicationContext
	{
		private enum _RunMode
		{
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
		}

		public IRunMode GetRunMode()
		{
			switch(_mode)
			{
				case _RunMode.Install:
					return new InstallRunMode();

				case _RunMode.Uninstall:
					return new UninstallRunMode();

				default:
					return new SelfHostRunMode(
						new OpenDrawingInOtherHostRunMode());
			}
		}

		public FileSystemFactory FileSystemFactory { get; }
		public RendererFactory RendererFactory { get; }
		public string FileName { get; }
		public string WorkingDirectory { get; }
		public DiagramFormat Format { get; set; }

		public void PrintDrawing(ShowDiagramStructure command)
		{
			var tempFile = _CreateTempFile();

			var renderer = RendererFactory.GetImageRenderer();
			var fileSystem = FileSystemFactory.GetFileSystem(null);
			var drawing = command.GetDrawing();

			using (var writeStream = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write))
				renderer.RenderDrawing(writeStream, new DrawingViewModel(drawing, fileSystem));

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
