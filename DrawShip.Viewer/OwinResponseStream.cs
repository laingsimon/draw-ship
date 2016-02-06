using System;
using System.IO;
using Microsoft.Owin;

namespace DrawShip.Viewer
{
	public class OwinResponseStream : Stream
	{
		private readonly IOwinResponse _response;

		public OwinResponseStream(IOwinResponse response)
		{
			_response = response;
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		public override void Flush()
		{ }

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			_response.ContentLength = value;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_response.Write(buffer, offset, count);
		}
	}
}
