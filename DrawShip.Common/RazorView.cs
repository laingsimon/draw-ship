using System.IO;
using RazorEngine.Templating;

namespace DrawShip.Common
{
	public class RazorView : ITemplateSource
	{
		public static readonly RazorView Drawing = new RazorView(Properties.Resources.Drawing);

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
