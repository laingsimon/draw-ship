using System.Runtime.InteropServices;

namespace DrawShip.ComInterop
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
	interface IInitializeWithFile
	{
		void Initialize([MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, uint grfMode);
	}
}