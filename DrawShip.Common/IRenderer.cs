namespace DrawShip.Common
{
    public interface IRenderer<T>
    {
        IRenderResult RenderDrawing(T data);
    }
}
