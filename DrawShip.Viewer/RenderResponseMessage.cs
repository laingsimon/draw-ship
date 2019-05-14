using DrawShip.Common;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DrawShip.Viewer
{
    public class RenderResponseMessage : IHttpActionResult
    {
        private readonly ApiController _controller;
        private readonly IRenderer<DrawingViewModel> _renderer;
        private readonly DrawingViewModel _viewModel;

        public RenderResponseMessage(IRenderer<DrawingViewModel> renderer, DrawingViewModel viewModel, ApiController controller)
        {
            _renderer = renderer;
            _viewModel = viewModel;
            _controller = controller;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var renderStream = new MemoryStream();
                var result = _renderer.RenderDrawing(_viewModel);

                result.WriteResult(renderStream);

                var responseStream = new MemoryStream(renderStream.ToArray());
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(responseStream)
                };
            }, cancellationToken);
        }
    }
}
