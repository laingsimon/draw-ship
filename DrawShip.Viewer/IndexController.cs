using System.Net;
using System.Web.Http;

namespace DrawShip.Viewer
{
    public class IndexController : ApiController
    {
        private readonly HostingContext _hostingContext;

        public IndexController()
        {
            _hostingContext = HostingContext.Instance;
        }

        public IHttpActionResult Get(int? directoryKey = null)
        {
            return new RenderViewActionResult(
                Properties.Resources.Index, 
                new IndexViewModel(_hostingContext, directoryKey), 
                HttpStatusCode.OK);
        }
    }
}
