using System.IO;
using System.Xml.Linq;

namespace DrawShip.Common
{
	public class Drawing
	{
		public Drawing(string fileName, string filePath)
		{
			FileName = fileName;
			FilePath = filePath;
		}

		public string FileName { get; }
		public string FilePath { get; }

		public string ReadDiagramContent()
		{
			var xml = XDocument.Load(Path.Combine(FilePath, FileName));
			var diagram = xml.Root.Element("diagram");

			return diagram.Value;
		}
	}
}
