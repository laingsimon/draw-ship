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

		public string FileName { get; }
		public string WorkingDirectory { get; }
		public DiagramFormat Format { get; private set; }
	}
}
