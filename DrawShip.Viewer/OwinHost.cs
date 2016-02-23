using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	/// <summary>
	/// The host for owin, this type will create and start the owin host
	/// </summary>
	public class OwinHost : IOwinHost
	{
		private readonly IDisposable _service;
		private readonly int _port;

		public OwinHost(int port)
		{
			_port = port;
			_service = WebApp.Start<_Service>(string.Format("http://localhost:{0}", port));
		}

		/// <summary>
		/// Stop the owin host
		/// </summary>
		public void Dispose()
		{
			_service.Dispose();
		}

		/// <summary>
		/// The port that owin is running on
		/// </summary>
		public int Port => _port;

		/// <summary>
		/// Owin infrastructure
		/// </summary>
		private class _Service
		{
			/// <summary>
			/// Configure and start owin
			/// </summary>
			/// <param name="app"></param>
			public void Configuration(IAppBuilder app)
			{
				app.UseErrorPage();

				var httpHandler = new Lazy<HttpHandler>(
					() => {
						var hostingContext = HostingContext.Instance;
						var applicationContext = hostingContext.ApplicationContext;
						var rendererFactory = applicationContext.RendererFactory;

						return new HttpHandler(
							hostingContext,
							applicationContext.FileSystemFactory,
							rendererFactory.GetHtmlRenderer(),
							rendererFactory.GetImageRenderer());
					});
				app.Run(context =>
				{
					return Task.Factory.StartNew(() => httpHandler.Value.Handle(context));
				});
			}
		}
	}
}
