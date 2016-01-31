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
		}

		public string FileName { get; private set; }
		public string WorkingDirectory { get; private set; }
		public Mutex Mutex { get; set; }

		public void StartProcess(string url)
		{
			Process.Start(url);
		}
	}
}
