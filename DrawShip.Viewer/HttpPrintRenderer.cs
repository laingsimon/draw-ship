using System.IO;
using DrawShip.Common;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	public class HttpPrintRenderer : IRenderer
	{
		private readonly ApplicationContext _applicationContext;

		public HttpPrintRenderer(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}

		public void RenderDrawing<T>(Stream outputStream, T viewModel)
			where T : IDrawingViewModel
		{
			var concreteViewModel = viewModel as DrawingViewModel;

			var task = Task.Factory.StartNew(() =>
			{
				_applicationContext.PrintDrawing(new ShowDiagramStructure
				{
					Format = DiagramFormat.Print,
					Directory = concreteViewModel.Drawing.FilePath,
					FileName = concreteViewModel.Drawing.FileName,
					Version = concreteViewModel.Version
				});
			});

			using (var writer = new StreamWriter(outputStream))
			{
				writer.WriteLine(@"
<html>
<body onload='window.history.back();'>
</body>
</html>");
			}
		}
	}
}
