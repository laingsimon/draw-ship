using System;

namespace DrawShip.Viewer.ComInterop
{
	[Flags]
	public enum MFMENU : uint
	{
		MF_UNCHECKED = 0,
		MF_STRING = 0,
		MF_ENABLED = 0,
		MF_BYCOMMAND = 0,
		MF_GRAYED = 1,
		MF_DISABLED = 0x00000002,
		MF_CHECKED = 0x00000008,
		MF_POPUP = 0x00000010,
		MF_HILITE = 0x00000080,
		MF_BYPOSITION = 0x00000400,
		MF_SEPARATOR = 0x00000800,
	}
}
