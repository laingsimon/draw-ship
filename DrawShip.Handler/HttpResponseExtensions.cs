using System.Net;
using System.Web;

namespace DrawShip.Handler
{
	public static class HttpResponseExtensions
	{
		public static void Respond(this HttpResponse response, HttpStatusCode statusCode, string message = null)
		{
			response.StatusCode = (int)statusCode;
			if (!string.IsNullOrEmpty(message))
				response.Write(message);
		}
	}
}
