using DrawShip.Common;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
    public class HttpPrintRenderer : IRenderer<DrawingViewModel>
    {
        private readonly ApplicationContext _applicationContext;

        public HttpPrintRenderer(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IRenderResult RenderDrawing(DrawingViewModel viewModel)
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

            return new StringRenderResult(@"
<html>
<body onload='window.history.back();'>
</body>
</html>");
        }
    }
}
