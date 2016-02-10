using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DrawShip.Viewer
{
	public class ApplicationContext
	{
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
		}

		public IRunMode GetRunMode()
		{
			return new SelfHostRunMode(
				new OpenDrawingInOtherHostRunMode());
		}

		public string FileName { get; private set; }
		public string WorkingDirectory { get; private set; }
		public DiagramFormat Format { get; private set; }
	}
}
