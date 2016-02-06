using System.IO;

namespace DrawShip.Common
{
	public interface IIRenderer
	{
		void RenderDrawing(Stream outputStream, DrawingViewModel viewModel);
	}
}
