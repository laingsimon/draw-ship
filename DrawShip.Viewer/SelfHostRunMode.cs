using System;
using System.Threading;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
	public class SelfHostRunMode : IRunMode
	{
		private static readonly Mutex _mutex = new Mutex(true, "A727D06E-77C3-4760-AC74-C1D76DD11B91");
		private readonly IRunMode _openInOtherInstance;

		public SelfHostRunMode(IRunMode openInOtherInstance)
		{
			_openInOtherInstance = openInOtherInstance;
		}

		public bool Run(ApplicationContext applicationContext)
		{
			if (!_mutex.WaitOne(TimeSpan.FromSeconds(1)))
			{
				var openedInOtherInstance = _openInOtherInstance.Run(applicationContext);
				if (openedInOtherInstance)
					return true;
			}

			try
			{
				using (var owinHost = new OwinHost(5142))
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					var form = new HostingDetail(
						new HostingContext(
							applicationContext,
							owinHost));
					Application.Run(form);
				}

				return true;
			}
			finally
			{
				_mutex.ReleaseMutex();
			}
		}
	}
}
