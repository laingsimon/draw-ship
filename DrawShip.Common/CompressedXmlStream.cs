using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DrawShip
{
	internal static class CompressedXmlStream
	{
		public static Stream Read(Stream stream)
		{
			var base64 = _GetBase64DiagramBlock(stream);
			if (string.IsNullOrEmpty(base64))
				return null;

			var decoded = new Base64Stream(base64);
			var decompression = new DeflateStream(decoded, CompressionMode.Decompress);
			return new UrlEncodingStream(decompression, true);
		}

		private static string _GetBase64DiagramBlock(Stream stream)
		{
			var xml = XDocument.Load(stream);
			return xml.XPathSelectElement("//diagram")?.Value;
		}

		public static Stream Read(string base64EncodedDeflatedXml)
		{
			if (string.IsNullOrEmpty(base64EncodedDeflatedXml))
				throw new ArgumentNullException("base64EncodedDeflatedXml");

			var decoded = new Base64Stream(base64EncodedDeflatedXml);
			var decompression = new DeflateStream(decoded, CompressionMode.Decompress);
			return new UrlEncodingStream(decompression, true);
		}

		public static Stream Write(Stream underlyingStream)
		{
			if (underlyingStream == null)
				throw new ArgumentNullException("underlyingStream");
			if (!underlyingStream.CanWrite)
				throw new ArgumentException("Stream must be writable", "underlyingStream");

			var base64Stream = new Base64Stream(underlyingStream, false);
			var compression = new DeflateStream(base64Stream, CompressionMode.Compress);
			return new UrlEncodingStream(compression, false);
		}
	}
}
