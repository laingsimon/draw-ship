using Microsoft.Owin;
using System.Net;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	public static class OwinExtensions
	{
		/// <summary>
		/// Extension method to assist with responding with a HTTP status code and a message
		/// </summary>
		/// <param name="context"></param>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static async Task Respond(this IOwinContext context, HttpStatusCode statusCode, string message)
		{
			context.Response.StatusCode = (int)statusCode;
			await context.Response.WriteAsync(message);
		}
	}
}
