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

		public void RenderDrawing(Stream outputStream, DrawingViewModel viewModel)
		{
			var task = Task.Factory.StartNew(() =>
			{
				_applicationContext.PrintDrawing(new ShowDiagramStructure
				{
					Format = DiagramFormat.Print,
					Directory = viewModel.Drawing.FilePath,
					FileName = viewModel.Drawing.FileName,
					Version = viewModel.Version
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
