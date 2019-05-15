using DrawShip.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using RazorEngine.Templating;

namespace DrawShip.Viewer
{
    internal class RenderViewActionResult : IHttpActionResult
    {
        private readonly string _viewSource;
        private readonly object _viewModel;
        private readonly HttpStatusCode _statusCode;

        public RenderViewActionResult(string viewSource, object viewModel, HttpStatusCode statusCode)
        {
            _viewSource = viewSource;
            _viewModel = viewModel;
            _statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_RenderView())
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("text/html")
                    }
                }
            });
        }

        private string _RenderView()
        {
            using (var textWriter = new StringWriter())
            {
                RazorEngine.Engine.Razor.RunCompile(
                    templateSource: new RazorView(_viewSource),
                    name: _viewModel.GetType().Name,
                    writer: textWriter,
                    modelType: _viewModel.GetType(),
                    model: _viewModel);

                return textWriter.GetStringBuilder().ToString();
            }
        }
    }
}
