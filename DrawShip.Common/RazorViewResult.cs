using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
    public class RazorViewResult : IRenderResult
    {
        private RazorView _view;
        private readonly object _viewModel;

        public RazorViewResult(RazorView view, object viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void WriteResult(Stream output)
        {
            using (var textWriter = new StreamWriter(output))
            {
                RazorEngine.Engine.Razor.RunCompile(
                    templateSource: _view,
                    name: "drawing",
                    writer: textWriter,
                    modelType: _viewModel.GetType(),
                    model: _viewModel);
            }
        }
    }
}
