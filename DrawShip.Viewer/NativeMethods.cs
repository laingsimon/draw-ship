using System;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	public static class NativeMethods
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[StructLayout(LayoutKind.Sequential)]
		public struct COPYDATASTRUCT
		{
			/// <summary>
			/// Any identifier
			/// </summary>
			public IntPtr dwData;

			/// <summary>
			/// The length of lpData
			/// </summary>
			public int cbData;

			/// <summary>
			/// Pointer to data
			/// </summary>
			public IntPtr lpData;

			/// <summary>
			/// Allocate a pointer to an arbitrary structure on the global heap.
			/// </summary>
			/// <returns></returns>
			public IntPtr AllocatePointer()
			{
				IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, retval, false);
				return retval;
			}
		}

		public const int WM_COPYDATA = 0x004A;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Free a pointer to an arbitrary structure from the global heap.
		/// </summary>
		/// <param name="ptr"></param>
		public static void FreePointer(this IntPtr ptr)
		{
			if (IntPtr.Zero == ptr)
				return;

			Marshal.FreeHGlobal(ptr);
			ptr = IntPtr.Zero;
		}
	}
}
