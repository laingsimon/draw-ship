using DrawShip.Common;
using System.Net.Http;

namespace DrawShip.Viewer
{
    /// <summary>
    ///  Get the relevant file system for the given request
    /// </summary>
    public class FileSystemFactory
    {
        public IFileSystem GetFileSystem(HttpRequestMessage request)
        {
            return new LocalFileSystem();
        }
    }
}
