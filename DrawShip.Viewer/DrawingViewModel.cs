using DrawShip.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DrawShip.Viewer
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

        public string PageNamesJson
        {
            get { return JsonConvert.SerializeObject(_drawing.GetPageNames(_fileSystem, _version).ToList()); }
        }

        public int PageCount
        {
            get { return _drawing.GetPageNames(_fileSystem, _version).Count(); }
        }

        public int PageIndex { get; set; }
        public string HighlightColour { get; set; } = ConfigurationManager.AppSettings["linkColour"] ?? "#0000ff";
        public List<string> ToolbarButtons { get; set; } = new List<string>(new[] { "pages", "zoom", "layers" });
        public bool Lightbox { get; set; }

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

        public string ReadDrawingData()
        {
            var data = new
            {
                highlight = HighlightColour,
                nav = true,
                resize = true,
                xml = ReadFileContent(),
                toolbar = string.Join(" ", ToolbarButtons),
                page = PageIndex,
                lightbox = Lightbox
            };

            return JsonConvert.SerializeObject(data, Formatting.None);
        }
    }
}
