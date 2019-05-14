using System;
using System.IO;

namespace DrawShip.Common
{
    /// <summary>
    /// A file system which reads from local or network resources
    /// </summary>
    public class LocalFileSystem : IFileSystem
    {
        public bool DirectoryExists(string physicalPath)
        {
            return Directory.Exists(physicalPath);
        }

        public bool FileExists(Drawing drawing)
        {
            var path = Path.Combine(drawing.FilePath, drawing.FileName);
            return File.Exists(path);
        }

        public Stream OpenRead(Drawing drawing, string version = null)
        {
            if (!string.IsNullOrEmpty(version))
                throw new NotSupportedException("The local filesystem does not support versioned documents");

            var path = Path.Combine(drawing.FilePath, drawing.FileName);
            return File.OpenRead(path);
        }
    }
}
