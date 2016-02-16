using DrawShip.Common;
using System;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Net.Http;

namespace DrawShip.Viewer
{
	public class RendererFactory
	{
		public ImageRenderer GetImageRenderer()
		{
			var imageExportUrl = new Uri("https://exp.draw.io/ImageExport4/export", UriKind.Absolute);
			var proxy = WebRequest.DefaultWebProxy.GetProxy(imageExportUrl);

			return new ImageRenderer(
				new HttpClient(new HttpClientHandler
				{
					Proxy = proxy != imageExportUrl ? new WebProxy(proxy) { UseDefaultCredentials = true } : null,
					UseProxy = proxy != null
				}),
				imageExportUrl,
				_GetImagePreviewSize());
		}

		public HtmlRenderer GetHtmlRenderer()
		{
			return new HtmlRenderer(RazorView.Drawing);
		}

		private static Size _GetImagePreviewSize()
		{
			int resolution = 3000;

			int.TryParse(ConfigurationManager.AppSettings["imageResolution"], out resolution);
			return new Size(
				resolution,
				resolution);
		}

	}
}
