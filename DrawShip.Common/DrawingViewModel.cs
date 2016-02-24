using System;
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
		private readonly Uri _imageFormatUri;
		private readonly string _version;
		private readonly Uri _printUri;

		public DrawingViewModel(Drawing drawing, IFileSystem fileSystem, Uri imageFormatUri, Uri printUri, string version = null)
		{
			_drawing = drawing;
			_fileSystem = fileSystem;
			_version = version;
			_imageFormatUri = imageFormatUri;
			_printUri = printUri;
		}

		public Drawing Drawing
		{
			get { return _drawing; }
		}

		public string Version
		{
			get { return _version; }
		}

		public Uri ImageFormatUri
		{
			get { return _imageFormatUri; }
		}

		public Uri PrintUri
		{
			get { return _printUri; }
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
