using System.IO;

namespace DrawShip.Common
{
	public interface IFileSystem
	{
		Stream OpenRead(Drawing drawing, string version = null);
		bool FileExists(Drawing drawing);
	}
}
