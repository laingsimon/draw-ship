﻿using DrawShip.Common;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;

namespace DrawShip.Handler
{
    /// <summary>
    /// ASP.net http handler for IIS, which can respond to requests with html rendered responses
    /// Image responses are not possible, as if the IIS instance is running behind a proxy, then the application
    ///   will not be able to communicate out (unless the app is running as a proxy-permitted user).
    /// </summary>
    public class HttpHandler : IHttpHandler
    {
        private readonly PathLibrary _pathLibrary;
        private readonly IRenderer<DrawingViewModel> _renderer;
        private readonly FileSystemFactory _fileSystemFactory;

        public HttpHandler()
        {
            _renderer = new HtmlRenderer<DrawingViewModel>(new RazorView(Properties.Resources.Drawing));
            _pathLibrary = new PathLibrary();
            _fileSystemFactory = new FileSystemFactory();
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

            var filePath = _GetApplicationRelativePath(request.ServerVariables, request.Url.LocalPath);
            var fileName = Path.GetFileName(filePath);
            var library = HttpUtility.UrlDecode(Path.GetDirectoryName(filePath));
            var physicalPath = _pathLibrary.GetPhysicalPath(library);
            var version = _GetVersion(request);
            var fileSystem = _fileSystemFactory.GetFileSystem(request);

            if (physicalPath == null)
            {
                response.Respond(HttpStatusCode.NotFound, "Library not found: " + library);
                return;
            }
            else if (!fileSystem.DirectoryExists(physicalPath))
            {
                response.Respond(HttpStatusCode.NotFound, "Library directory does not exist: " + library + " --> " + physicalPath);
                return;
            }

            var drawing = new Drawing(Path.ChangeExtension(fileName, ".xml"), physicalPath);

            if (!fileSystem.FileExists(drawing))
            {
                response.Respond(HttpStatusCode.NotFound, "Drawing not found: " + drawing.FileName);
                return;
            }

            var viewModel = new DrawingViewModel(
                drawing,
                fileSystem,
                version);
            var result = _renderer.RenderDrawing(viewModel);

            result.WriteResult(response.OutputStream);
        }

        private string _GetVersion(HttpRequest request)
        {
            return request.QueryString["v"];
        }

        private string _GetApplicationRelativePath(NameValueCollection serverVariables, string pathAndQuery)
        {
            var applicationName = "/" + Path.GetFileName(serverVariables["APPL_MD_PATH"]) + "/";
            return HttpUtility.UrlDecode(pathAndQuery.Substring(applicationName.Length));
        }
    }
}
