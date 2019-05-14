using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
    /// <summary>
    /// A type which represents a razor view
    /// </summary>
    public class RazorView : ITemplateSource
    {
        private readonly byte[] _content;

        public RazorView(byte[] content)
        {
            _content = content;
        }

        public string Template => null;
        public string TemplateFile => null;

        public TextReader GetTemplateReader()
        {
            return new StreamReader(new MemoryStream(_content));
        }
    }
}
