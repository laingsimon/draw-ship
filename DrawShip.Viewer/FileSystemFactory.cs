using DrawShip.Common;
using Microsoft.Owin;

namespace DrawShip.Viewer
{
	/// <summary>
	///  Get the relevant file system for the given request
	/// </summary>
	public class FileSystemFactory
	{
		public IFileSystem GetFileSystem(IOwinContext context)
		{
			return new LocalFileSystem();
		}
	}
}
