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
		private static int _port;

		public OwinHost(int port)
		{
			_port = port;
			_service = WebApp.Start<_Service>(string.Format("http://localhost:{0}", port));
		}

		public void Dispose()
		{
			_service.Dispose();
		}

		public int Port => _port;

		private class _Service
		{
			public void Configuration(IAppBuilder app)
			{
				_port = _GetRunningPort(app);

				app.UseErrorPage();

				var httpHandler = new Lazy<HttpHandler>(() => new HttpHandler(HostingContext.Instance));
				app.Run(context =>
				{
					return Task.Factory.StartNew(() => httpHandler.Value.Handle(context));
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
