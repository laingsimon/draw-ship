using Microsoft.Owin;
using System.Net;
using System.Threading.Tasks;

namespace DrawShip.Viewer
{
	public static class OwinExtensions
	{
		public static async Task Respond(this IOwinContext context, HttpStatusCode statusCode, string message)
		{
			context.Response.StatusCode = (int)statusCode;
			await context.Response.WriteAsync(message);
		}
	}
}
