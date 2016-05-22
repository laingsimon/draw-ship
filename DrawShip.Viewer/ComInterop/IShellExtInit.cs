using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer.ComInterop
{
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e8-0000-0000-c000-000000000046")]
	public interface IShellExtInit
	{
		[PreserveSig]
		int Initialize (IntPtr pidlFolder, IntPtr lpdobj, uint /*HKEY*/ hKeyProgID);
	}
}
