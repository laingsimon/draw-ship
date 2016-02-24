using System.IO;

namespace DrawShip.Common
{
	public interface IRenderer
	{
		void RenderDrawing<T>(Stream outputStream, T viewModel)
			where T : IDrawingViewModel;
	}
}
