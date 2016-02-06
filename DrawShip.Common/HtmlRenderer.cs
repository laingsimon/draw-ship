using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
	public class HtmlRenderer : IIRenderer
	{
		private readonly RazorView _razorView;

		public HtmlRenderer(RazorView razorView)
		{
			_razorView = razorView;
		}

		public void RenderDrawing(Stream outputStream, DrawingViewModel viewModel)
		{
			using (var textWriter = new StreamWriter(outputStream))
			{
				RazorEngine.Engine.Razor.RunCompile(
					templateSource: _razorView,
					name: "drawing",
					writer: textWriter,
					modelType: typeof(DrawingViewModel),
					model: viewModel);
			}
		}
	}
}
