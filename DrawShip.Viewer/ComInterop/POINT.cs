using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct POINT
	{
		public int X;
		public int Y;
	}
}
