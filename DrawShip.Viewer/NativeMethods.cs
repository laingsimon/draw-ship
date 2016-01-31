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
		}

		public const int WM_COPYDATA = 0x004A;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
	}
}
