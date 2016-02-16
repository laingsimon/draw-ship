using System.IO;

namespace DrawShip.Common
{
	public interface IRenderer
	{
		void RenderDrawing(Stream outputStream, DrawingViewModel viewModel);
	}
}
