using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CMINVOKECOMMANDINFO
	{
		public uint cbSize;
		public CMIC fMask;
		public IntPtr hwnd;
		public IntPtr verb;
		[MarshalAs(UnmanagedType.LPStr)]
		public string parameters;
		[MarshalAs(UnmanagedType.LPStr)]
		public string directory;
		public int nShow;
		public uint dwHotKey;
		public IntPtr hIcon;
	}
}
