using DrawShip.Common;
using Newtonsoft.Json;
using System;

namespace DrawShip.Viewer
{
    public class DrawingDetailsRenderer : IRenderer<DrawingViewModel>
    {
        public IRenderResult RenderDrawing(DrawingViewModel data)
        {
            return new StringRenderResult(JsonConvert.SerializeObject(new {
                lastWriteTime = data.LastWriteTime.ToString("u")
            }));
        }
    }
}
