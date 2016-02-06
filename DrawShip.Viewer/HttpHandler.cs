using Microsoft.Owin;
using System.Net;
using DrawShip.Common;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
			var fileNameAndDirectoryKey = context.Request.Path.Value;
			var match = Regex.Match(fileNameAndDirectoryKey, @"^\/(?<directoryKey>.+?)\/(?<fileName>.+?)$");
			if (!match.Success)
			{
				await context.Respond(HttpStatusCode.BadRequest, "Invalid request - directoryKey and fileName cannot be extracted");
				return;
			}

			var fileName = match.Groups["fileName"].Value;
			var directoryKey = match.Groups["directoryKey"].Value;
			var directory = _hostingContext.GetDirectory(directoryKey);

			if (directory == null)
			{
				await context.Respond(HttpStatusCode.NotFound, "Directory not known - " + directoryKey);
				return;
			}

			var version = _GetVersion(context.Request.QueryString);
			var command = new ShowDiagramStructure
			{
				FileName = fileName,
				Directory = directory,
				Version = version
			};

			var drawing = new Drawing(fileName, command.Directory);
			if (!File.Exists(Path.Combine(drawing.FilePath, drawing.FileName)))
			{
				await context.Respond(HttpStatusCode.NotFound, "Drawing not : " + drawing.FileName);
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