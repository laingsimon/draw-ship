using System.Net;
using System.Web;

namespace DrawShip.Handler
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Extension method to assist with responding with a HTTP status code and an optional message
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public static void Respond(this HttpResponse response, HttpStatusCode statusCode, string message = null)
        {
            response.StatusCode = (int)statusCode;
            if (!string.IsNullOrEmpty(message))
                response.Write(message);
        }
    }
}
