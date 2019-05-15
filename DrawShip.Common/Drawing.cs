namespace DrawShip.Common
{
    /// <summary>
    /// A drawing
    /// </summary>
    public class Drawing
    {
        public static string[] permittedExtensions = { ".xml", ".drawio" };

        public Drawing(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }

        /// <summary>
        /// The name of the file that contains the drawing
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The path to the directory that contains the drawing
        /// </summary>
        public string FilePath { get; }
    }
}
