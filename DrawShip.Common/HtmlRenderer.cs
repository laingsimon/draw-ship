namespace DrawShip.Common
{
	/// <summary>
	/// Renderer that can render the drawing in a html interactive preview compatible manner
	/// </summary>
	public class HtmlRenderer<T> : IRenderer<T>
	{
		private readonly RazorView _razorView;

		public HtmlRenderer(RazorView razorView)
		{
			_razorView = razorView;
		}

		public IRenderResult RenderDrawing(T data)
		{
			return new RazorViewResult(_razorView, data);
		}
	}
}
