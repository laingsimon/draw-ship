using System;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
	public class ApplicationContext
	{
		private enum RunMode
		{
			Run,
			Install,
			Uninstall
		}

		private readonly RunMode _mode = RunMode.Run;

		public ApplicationContext()
		{
			var commandLine = Environment.GetCommandLineArgs().Skip(1).ToArray();
			FileName = commandLine.FirstOrDefault(a => !a.StartsWith("/"));
			if (FileName != null)
				WorkingDirectory = Path.GetDirectoryName(FileName);

			var configuredDir = commandLine.SingleOrDefault(arg => arg.StartsWith("/dir:"));
			if (configuredDir != null)
				WorkingDirectory = configuredDir.Substring("/dir:".Length);

			if (string.IsNullOrEmpty(WorkingDirectory))
				WorkingDirectory = Environment.CurrentDirectory;

			var formatString = commandLine.SingleOrDefault(arg => arg.StartsWith("/format:"));
			if (formatString != null)
			{
				var format = DiagramFormat.Html;
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
				case RunMode.Install:
					return new InstallRunMode();

				case RunMode.Uninstall:
					return new UninstallRunMode();

				default:
					return new SelfHostRunMode(
						new OpenDrawingInOtherHostRunMode());
			}
		}

		public string FileName { get; private set; }
		public string WorkingDirectory { get; private set; }
		public DiagramFormat Format { get; private set; }
	}
}
