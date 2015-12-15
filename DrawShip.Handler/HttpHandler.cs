using System;
using System.Diagnostics;
using System.Web;

namespace DrawShip.Handler
{
	public class HttpHandler : IHttpHandler
	{
		public bool IsReusable
		{
			[DebuggerStepThrough]
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var request = context.Request;
			var response = context.Response;

			response.StatusCode = 200;
			response.Write("hello world" + request.Url);
		}
	}
}
