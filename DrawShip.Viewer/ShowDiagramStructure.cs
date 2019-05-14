using DrawShip.Common;
using System.IO;

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

        public Drawing GetDrawing(string fileName = null)
        {
            return new Drawing(Path.ChangeExtension(fileName ?? FileName, ".xml"), Directory);
        }
    }
}
