using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DrawShip.Viewer.ComInterop
{
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e4-0000-0000-c000-000000000046")]
	public interface IContextMenu
	{
		// IContextMenu methods
		[PreserveSig]
		int QueryContextMenu(IntPtr hmenu, int iMenu, int idCmdFirst, int idCmdLast, CMF uFlags);
		[PreserveSig]
		void InvokeCommand(IntPtr pici);
		[PreserveSig]
		void GetCommandString(int idcmd, uint uflags, int reserved, StringBuilder commandstring, int cch);
	}
}
