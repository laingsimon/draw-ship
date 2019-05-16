using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            return _GetFileNames(drawing).Any(File.Exists);
        }

        public DateTime GetLastWriteTime(Drawing drawing, string version)
        {
            return _GetFileNames(drawing)
                .Where(File.Exists)
                .Select(File.GetLastWriteTimeUtc)
                .FirstOrDefault();
        }

        private IEnumerable<string> _GetFileNames(Drawing drawing)
        {
            if (Path.GetExtension(drawing.FileName) != "")
                return new[] { Path.Combine(drawing.FilePath, drawing.FileName) };

            return Drawing.permittedExtensions.Select(extension => Path.ChangeExtension(drawing.FileName, extension));
        }

        public Stream OpenRead(Drawing drawing, string version = null)
        {
            if (!string.IsNullOrEmpty(version))
                throw new NotSupportedException("The local filesystem does not support versioned documents");

            var path = _GetFileNames(drawing)
                .Where(File.Exists)
                .Single();
            return File.OpenRead(path);
        }
    }
}
