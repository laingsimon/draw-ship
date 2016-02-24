using System.IO;

namespace DrawShip.Common
{
	public interface IRenderResult
	{
		void WriteResult(Stream output);
	}
}
