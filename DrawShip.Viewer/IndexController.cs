using DrawShip.Common;
using RazorEngine.Templating;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace DrawShip.Viewer
{
	public class IndexController : ApiController
	{
		private readonly HostingContext _hostingContext;

		public IndexController()
		{
			_hostingContext = HostingContext.Instance;
		}

		public HttpResponseMessage Get()
		{
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(_RenderHtml())
				{
					Headers =
					{
						ContentType = new MediaTypeHeaderValue("text/html")
					}
				}
			};
		}

		private string _RenderHtml()
		{
			var view = new RazorView(Properties.Resources.Index);
			var viewModel = new IndexViewModel(_hostingContext);

			using (var textWriter = new StringWriter())
			{
				RazorEngine.Engine.Razor.RunCompile(
					templateSource: view,
					name: "index",
					writer: textWriter,
					modelType: typeof(IndexViewModel),
					model: viewModel);

				return textWriter.GetStringBuilder().ToString();
			}
		}
	}
}
