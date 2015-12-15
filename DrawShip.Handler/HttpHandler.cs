using DrawShip.Common;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;

namespace DrawShip.Handler
{
	public class HttpHandler : IHttpHandler
	{
		private readonly PathLibrary _pathLibrary;
		private readonly HtmlRenderer _renderer;

		public HttpHandler()
		{
			_renderer = new HtmlRenderer(RazorView.Drawing);
			_pathLibrary = new PathLibrary();
		}

		public bool IsReusable
		{
			[DebuggerStepThrough]
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var request = context.Request;
			var response = context.Response;

			var filePath = _GetApplicationRelativePath(request.ServerVariables, request.Url.PathAndQuery);
			var fileName = Path.GetFileName(filePath);
			var library = HttpUtility.UrlDecode(Path.GetDirectoryName(filePath));
			var physicalPath = _pathLibrary.GetPhysicalPath(library);

			if (physicalPath == null)
			{
				response.Respond(HttpStatusCode.NotFound, "Library not found: " + library);
				return;
			}
			else if (!Directory.Exists(physicalPath))
			{
				response.Respond(HttpStatusCode.NotFound, "Library directory does not exist: " + library + " --> " + physicalPath);
				return;
			}

			var drawing = new Drawing(fileName, physicalPath);

			if (!File.Exists(Path.Combine(drawing.FilePath, drawing.FileName)))
			{
				response.Respond(HttpStatusCode.NotFound, "Drawing not found: " + drawing.FileName);
				return;
			}

			_renderer.RenderDrawing(response.Output, drawing);
		}

		private string _GetApplicationRelativePath(NameValueCollection serverVariables, string pathAndQuery)
		{
			var applicationName = "/" + Path.GetFileName(serverVariables["APPL_MD_PATH"]) + "/";
			return HttpUtility.UrlDecode(pathAndQuery.Substring(applicationName.Length));
		}
	}
}
