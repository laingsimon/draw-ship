using DrawShip.Common;
using System.Web;

namespace DrawShip.Handler
{
    /// <summary>
    /// Get the relevant file system for the given http request
    /// </summary>
    public class FileSystemFactory
    {
        private readonly IFileSystem _defaultFileSystem;

        public FileSystemFactory()
        {
            _defaultFileSystem = new LocalFileSystem();
        }

        public IFileSystem GetFileSystem(HttpRequest request)
        {
            return _defaultFileSystem;
        }
    }
}
