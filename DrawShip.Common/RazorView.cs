using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
    /// <summary>
    /// A type which represents a razor view
    /// </summary>
    public class RazorView : ITemplateSource
    {
        private readonly string _content;

        public RazorView(string content)
        {
            _content = content;
        }

        public string Template => null;
        public string TemplateFile => null;

        public TextReader GetTemplateReader()
        {
            return new StringReader(_content);
        }
    }
}
