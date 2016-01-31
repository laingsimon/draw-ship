using DrawShip.Common;
using System.Web;

namespace DrawShip.Handler
{
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
