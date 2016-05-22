using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct MENUITEMINFO
	{
		public uint cbSize;
		public MIIM fMask;
		public MFT fType;
		public MFS fState;
		public uint wID;
		public IntPtr hSubMenu;
		public IntPtr hbmpChecked;
		public IntPtr hbmpUnchecked;
		public UIntPtr dwItemData;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string dwTypeData;
		public uint cch;
		public IntPtr hbmpItem;
	}
}
