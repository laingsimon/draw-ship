using System.Drawing;
using System.Runtime.InteropServices;

namespace DrawShip.Common.ComInterop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public readonly int left;
        public readonly int top;
        public readonly int right;
        public readonly int bottom;
        public Rectangle ToRectangle() { return Rectangle.FromLTRB(left, top, right, bottom); }
    }
}
