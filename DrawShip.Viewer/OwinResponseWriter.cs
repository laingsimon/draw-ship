using Microsoft.Owin;
using System.IO;
using System.Text;

namespace DrawShip.Viewer
{
	public class OwinResponseWriter : TextWriter
	{
		private readonly IOwinResponse _response;

		public OwinResponseWriter(IOwinResponse response)
		{
			_response = response;
		}

		public override void Write(string value)
		{
			_response.Write(value);
		}

		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}
	}
}
