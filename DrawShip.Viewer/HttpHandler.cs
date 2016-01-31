using Microsoft.Owin;
using System.Net;
using DrawShip.Common;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	public class HttpHandler
	{
		private readonly FileSystemFactory _fileSystemFactory;
		private readonly HostingContext _hostingContext;
		private readonly HtmlRenderer _renderer;

		public HttpHandler(HostingContext hostingContext)
		{
			_hostingContext = hostingContext;
			_renderer = new HtmlRenderer(RazorView.Drawing);
			_fileSystemFactory = new FileSystemFactory();
		}

		public async Task Handle(IOwinContext context)
		{
			var fileName = context.Request.Path.Value.TrimStart('/');
			var version = _GetVersion(context.Request.QueryString);

			var drawing = new Drawing(fileName, _hostingContext.ApplicationContext.WorkingDirectory);

			if (!File.Exists(Path.Combine(drawing.FilePath, drawing.FileName)))
			{
				await context.Respond(HttpStatusCode.NotFound, "Drawing not found: " + drawing.FileName);
				return;
			}

			var viewModel = new DrawingViewModel(
				drawing,
				_fileSystemFactory.GetFileSystem(context),
				version);
			_renderer.RenderDrawing(new OwinResponseWriter(context.Response), viewModel);
		}

		private string _GetVersion(QueryString owinQueryString)
		{
			var queryString = HttpUtility.ParseQueryString(owinQueryString.Value);
			return queryString["v"];
		}
	}
}