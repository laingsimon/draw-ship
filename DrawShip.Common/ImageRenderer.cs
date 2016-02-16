using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;

namespace DrawShip.Common
{
	public class ImageRenderer : IIRenderer
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

		public void RenderDrawing(Stream outputStream, DrawingViewModel viewModel)
		{
			var request = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "filename", "preview" },
				{ "format", "png" },
				{ "xml", _ReadFileContent(viewModel) },
				{ "base64", "O" },
				{ "bg", "none" },
				{ "w", _renderSize.Width.ToString() },
				{ "h", _renderSize.Height.ToString() },
				{ "border", "1" }
			});

			var response = _client.PostAsync(
				_requestUri,
				request).Result;

			var stream = response.Content.ReadAsStreamAsync().Result;
			stream.CopyTo(outputStream);
		}

		private string _ReadFileContent(DrawingViewModel viewModel)
		{
			return viewModel.ReadFileContent();
		}
	}
}
