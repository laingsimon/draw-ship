using System;
using System.IO;

namespace DrawShip.Common
{
	public class LocalFileSystem : IFileSystem
	{
		public Stream OpenRead(Drawing drawing, string version = null)
		{
			if (!string.IsNullOrEmpty(version))
				throw new NotSupportedException("The local filesystem does not support versioned documents");

			var path = Path.Combine(drawing.FilePath, drawing.FileName);
			return File.OpenRead(path);
		}
	}
}
