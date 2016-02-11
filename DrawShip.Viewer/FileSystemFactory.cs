using DrawShip.Common;
using Microsoft.Owin;

namespace DrawShip.Viewer
{
	public class FileSystemFactory
	{
		public IFileSystem GetFileSystem(IOwinContext context)
		{
			return new LocalFileSystem();
		}
	}
}
