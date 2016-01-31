using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	public class OwinHost : IOwinHost
	{
		private readonly IDisposable _service;
		private static int? _port;

		public OwinHost(int? port = null)
		{
			_port = port;
			var portArg = port.HasValue
				? string.Format(":{0}", port.Value)
				: "";
			_service = WebApp.Start<_Service>(string.Format("http://localhost{0}", portArg));
		}

		public void Dispose()
		{
			_service.Dispose();
		}

		public int Port
		{
			get
			{
				return _port ?? 80;
			}
		}

		private class _Service
		{
			public void Configuration(IAppBuilder app)
			{
				_port = _GetRunningPort(app);

				app.UseErrorPage();

				app.Run(context =>
				{
					var httpHandler = new HttpHandler(HostingContext.Instance);
					return Task.Factory.StartNew(() => httpHandler.Handle(context));
				});
			}

			private static int _GetRunningPort(IAppBuilder app)
			{
				var hostAddresses = (IEnumerable<IDictionary<string, object>>)app.Properties["host.Addresses"];
				var hostDictionary = hostAddresses.Single();

				return int.Parse((string)hostDictionary["port"]);
			}
		}
	}
}
