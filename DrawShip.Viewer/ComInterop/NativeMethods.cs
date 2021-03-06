﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DrawShip.Viewer.ComInterop
{
	internal static class NativeMethods
	{
		/// <summary>
		/// Retrieve the names of dropped files that result from a successful drag-
		/// and-drop operation.
		/// </summary>
		/// <param name="hDrop">
		/// Identifier of the structure that contains the file names of the dropped
		/// files.
		/// </param>
		/// <param name="iFile">
		/// Index of the file to query. If the value of this parameter is 0xFFFFFFFF,
		/// DragQueryFile returns a count of the files dropped.
		/// </param>
		/// <param name="pszFile">
		/// The address of a buffer that receives the file name of a dropped file
		/// when the function returns.
		/// </param>
		/// <param name="cch">
		/// The size, in characters, of the pszFile buffer.
		/// </param>
		/// <returns>A non-zero value indicates a successful call.</returns>
		[DllImport("shell32", CharSet = CharSet.Unicode)]
		public static extern uint DragQueryFile(
			IntPtr hDrop,
			uint iFile,
			StringBuilder pszFile,
			int cch);

		/// <summary>
		/// Insert a new menu item at the specified position in a menu.
		/// </summary>
		/// <param name="hMenu">
		/// A handle to the menu in which the new menu item is inserted.
		/// </param>
		/// <param name="uItem">
		/// The identifier or position of the menu item before which to insert the
		/// new item. The meaning of this parameter depends on the value of
		/// fByPosition.
		/// </param>
		/// <param name="fByPosition">
		/// Controls the meaning of uItem. If this parameter is false, uItem is a
		/// menu item identifier. Otherwise, it is a menu item position.
		/// </param>
		/// <param name="mii">
		/// A reference of a MENUITEMINFO structure that contains information about
		/// the new menu item.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is true.
		/// </returns>
		[DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InsertMenuItem(
			IntPtr hMenu,
			uint uItem,
			[MarshalAs(UnmanagedType.Bool)]bool fByPosition,
			ref MENUITEMINFO mii);

		public static int HighWord(int number)
		{
			return ((number & 0x80000000) == 0x80000000) ?
				(number >> 16) : ((number >> 16) & 0xffff);
		}

		public static int LowWord(int number)
		{
			return number & 0xffff;
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32")]
		internal static extern IntPtr CreatePopupMenu();

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

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(int nIndex);

		[DllImport("kernel32.dll")]
		public static extern bool RtlMoveMemory(IntPtr dest, IntPtr source, int dwcount);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, [In, MarshalAs(UnmanagedType.LPStruct)]BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
	}
}
