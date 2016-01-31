using System;
using System.Diagnostics;

namespace DrawShip.Viewer
{
	public class HostingContext
	{
		private static HostingContext _instance;

		private readonly ApplicationContext _applicationContext;
		private readonly IOwinHost _host;

		public HostingContext(ApplicationContext applicationContext, IOwinHost host)
		{
			if (_instance != null)
				throw new InvalidOperationException("Not allowed to have multiple instances");

			_applicationContext = applicationContext;
			_host = host;
			_instance = this;
		}

		public static HostingContext Instance
		{
			[DebuggerStepThrough]
			get { return _instance; }
		}

		public int Port
		{
			[DebuggerStepThrough]
			get { return _host.Port; }
		}

		public ApplicationContext ApplicationContext
		{
			[DebuggerStepThrough]
			get { return _applicationContext; }
		}

		public void ApplicationStarted()
		{
			DisplayDrawing(_applicationContext.FileName, "");
		}

		public void DisplayDrawing(string fileName, string version)
		{
			var versionQueryString = string.IsNullOrEmpty(version)
				? ""
				: "?v=" + version;
			var fileNameAndVersion = string.Format("{0}{1}", fileName, version);
			var url = string.Format(
				"http://localhost:{0}/{1}",
				Port,
				fileNameAndVersion);

			ApplicationContext.StartProcess(url);
		}

		public void DisplayIndex()
		{
			var url = string.Format(
				"http://localhost:{0}/",
				Port);

			ApplicationContext.StartProcess(url);
		}
	}
}
