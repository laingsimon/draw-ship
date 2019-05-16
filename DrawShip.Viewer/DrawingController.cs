using DrawShip.Common;
using System.Net;
using System.Web.Http;

namespace DrawShip.Viewer
{
    public class DrawingController : ApiController
    {
        private readonly FileSystemFactory _fileSystemFactory;
        private readonly HostingContext _hostingContext;
        private readonly RendererFactory _rendererFactory;

        public DrawingController()
        {
            _hostingContext = HostingContext.Instance;
            _fileSystemFactory = new FileSystemFactory();
            _rendererFactory = new RendererFactory();
        }

        public IHttpActionResult Get(int directoryKey, string fileName, string version = null, DiagramFormat format = DiagramFormat.Html)
        {
            var directory = _hostingContext.GetDirectory(directoryKey);

            if (directory == null)
                return new RenderViewActionResult(
                    Properties.Resources.Index,
                    new IndexViewModel(_hostingContext, directoryKey),
                    HttpStatusCode.NotFound);

            var command = new ShowDiagramStructure
            {
                FileName = fileName,
                Directory = directory,
                Version = version,
                Format = format
            };

            var drawing = command.GetDrawing(fileName);
            var fileSystem = _fileSystemFactory.GetFileSystem(Request);
            if (!fileSystem.FileExists(drawing))
                return new RenderViewActionResult(
                    Properties.Resources.NotFound, 
                    new NotFoundViewModel(_hostingContext, directoryKey, fileName), 
                    HttpStatusCode.NotFound);

            var viewModel = new DrawingViewModel(
                drawing,
                fileSystem,
                WebApiStartup.FormatUrl(directoryKey, fileName, DiagramFormat.Image, version),
                WebApiStartup.FormatUrl(directoryKey, fileName, DiagramFormat.Print, version),
                command.Version);
            var renderer = _GetRenderer(command.Format);

            return new RenderResponseMessage(renderer, viewModel, this);
        }

        /// <summary>
        /// Get the renderer for the current request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IRenderer<DrawingViewModel> _GetRenderer(DiagramFormat format)
        {
            switch (format)
            {
                case DiagramFormat.Image:
                    return _rendererFactory.GetImageRenderer();
                case DiagramFormat.Print:
                    return _rendererFactory.GetHttpPrintRenderer(_hostingContext.ApplicationContext);
                case DiagramFormat.Details:
                    return _rendererFactory.GetDetailsRenderer();
                default:
                    return _rendererFactory.GetHtmlRenderer();
            }
        }
    }
}
