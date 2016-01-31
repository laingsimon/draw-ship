using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DrawShip.Common
{
	public class DrawingViewModel
	{
		private readonly Drawing _drawing;
		private readonly IFileSystem _fileSystem;
		private readonly string _version;

		public DrawingViewModel(Drawing drawing, IFileSystem fileSystem, string version = null)
		{
			_drawing = drawing;
			_fileSystem = fileSystem;
			_version = version;
		}

		public string ShapeNames
		{
			get
			{
				var containedShapeNames = DiagramReader.GetContainedShapeNames(_drawing, _fileSystem, _version);
				return string.Join(";", containedShapeNames);
			}
		}

		public string DrawingTitle
		{
			get
			{
				var drawingName = Path.GetFileNameWithoutExtension(_drawing.FileName);

				return string.IsNullOrEmpty(_version)
					? drawingName
					: drawingName + " - v" + _version;
			}
		}

		public string ReadDiagramContent()
		{
			using (var stream = _fileSystem.OpenRead(_drawing, _version))
			{
				var xml = XDocument.Load(stream);
				var diagram = xml.Root.Element("diagram");

				return diagram?.Value;
			}
		}
	}
}
