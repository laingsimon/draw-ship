using System.IO;
using System.Xml.Linq;

namespace DrawShip.Common
{
	public class DrawingViewModel
	{
		private readonly Drawing _drawing;
		private readonly IFileSystem _fileSystem;
		private string[] _configuredShapeNames;
		private readonly string _version;

		public DrawingViewModel(Drawing drawing, IFileSystem fileSystem, string version = null, string[] configuredShapeNames = null)
		{
			_drawing = drawing;
			_fileSystem = fileSystem;
			_version = version;
			_configuredShapeNames = configuredShapeNames;
		}

		public string ShapeNames
		{
			get
			{
				if (_configuredShapeNames == null)
					return string.Empty;

				return string.Join(";", _configuredShapeNames);
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
