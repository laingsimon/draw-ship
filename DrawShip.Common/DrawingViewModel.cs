using System.IO;
using System.Xml.Linq;

namespace DrawShip.Common
{
	/// <summary>
	/// A view model representation of a drawing
	/// </summary>
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

		/// <summary>
		/// The names of the shapes that are used in the drawing
		/// </summary>
		public string ShapeNames
		{
			get
			{
				var containedShapeNames = _drawing.GetContainedShapeNames(_fileSystem, _version);
				return string.Join(";", containedShapeNames);
			}
		}

		/// <summary>
		/// The title for the drawing
		/// </summary>
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

		/// <summary>
		/// Read the drawing file content
		/// </summary>
		/// <returns></returns>
		public string ReadFileContent()
		{
			using (var stream = _fileSystem.OpenRead(_drawing, _version))
			using (var reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}

		/// <summary>
		/// Read the relevant part of the drawing
		/// </summary>
		/// <returns></returns>
		public string ReadDrawingContent()
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
