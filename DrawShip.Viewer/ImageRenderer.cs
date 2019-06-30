using DrawShip.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;

namespace DrawShip.Viewer
{
    /// <summary>
    /// Renderer that can render the drawing into an image (png)
    /// </summary>
    public class ImageRenderer : IRenderer<DrawingViewModel>
    {
        private readonly HttpClient _client;
        private readonly Size _renderSize;
        private readonly Uri _requestUri;

        public ImageRenderer(HttpClient client, Uri requestUri, Size renderSize)
        {
            _client = client;
            _requestUri = requestUri;
            _renderSize = renderSize;
        }

        public IRenderResult RenderDrawing(DrawingViewModel viewModel)
        {
            var request = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "filename", "preview" },
                { "format", "png" },
                { "xml", viewModel.ReadFileContent() },
                { "base64", "O" },
                { "bg", "#ffffff" },
                { "w", _renderSize.Width.ToString() },
                { "h", _renderSize.Height.ToString() },
                { "border", "1" },
                { "pageId", viewModel.GetPageId(viewModel.PageIndex) }
            });

            var response = _client.PostAsync(
                _requestUri,
                request).Result;

            var stream = response.Content.ReadAsStreamAsync().Result;
            return new StreamRenderResult(stream);
        }
    }
}
