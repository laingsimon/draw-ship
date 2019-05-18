using DrawShip.Common;
using System;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
    /// <summary>
    /// Information about a drawing which should be rendered.
    /// The name, format and path, etc.
    /// </summary>
    public struct ShowDiagramStructure
    {
        public string FileName { get; set; }
        public string Version { get; set; }
        public string Directory { get; set; }
        public DiagramFormat Format { get; set; }
        public int PageIndex { get; set; }

        public Drawing GetDrawing(string fileName = null)
        {
            fileName = fileName ?? FileName;

            if (string.IsNullOrEmpty(fileName))
                return null;

            if (Path.GetExtension(fileName) != "" && !Drawing.permittedExtensions.Any(ext => _ExtensionMatches(fileName, ext)))
                return null;

            return new Drawing(fileName, Directory);
        }

        private static bool _ExtensionMatches(string fileName, string permittedExtension)
        {
            return Path.GetExtension(fileName).Equals(permittedExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
