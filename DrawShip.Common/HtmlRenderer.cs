using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
	public class HtmlRenderer
	{
		private readonly RazorView _razorView;

		public HtmlRenderer(RazorView razorView)
		{
			_razorView = razorView;
		}

		public void RenderDrawing(TextWriter textWriter, DrawingViewModel viewModel)
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
