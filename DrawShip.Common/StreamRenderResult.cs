using System.IO;

namespace DrawShip.Common
{
	public class StreamRenderResult : IRenderResult
	{
		private readonly Stream _output;

		public StreamRenderResult(Stream output)
		{
			_output = output;
		}

		public void WriteResult(Stream output)
		{
			_output.CopyTo(output);
		}
	}
}
