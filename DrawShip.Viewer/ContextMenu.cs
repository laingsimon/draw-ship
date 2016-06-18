using DrawShip.Viewer.ComInterop;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
	public class ContextMenu
	{
		private readonly IntPtr _menu;
		private int _itemsAdded = 0;

		public ContextMenu(IntPtr menu)
		{
			_menu = menu;
		}

		public ContextMenu CreateSubMenu(string text, int position, Bitmap icon)
		{
			var submenu = NativeMethods.CreatePopupMenu();
			var handleToIcon = _ResizeBitmap(icon, _GetRequiredSize()).GetHbitmap();
			var menuItemInfo = new MENUITEMINFO
			{
				fType = MFT.MFT_BITMAP | MFT.MFT_STRING,
				fMask = MIIM.MIIM_BITMAP | MIIM.MIIM_STRING | MIIM.MIIM_SUBMENU,
				hbmpItem = handleToIcon,
				dwTypeData = text,
				cch = (uint)text.Length,
				hSubMenu = submenu
			};
			menuItemInfo.cbSize = (uint)Marshal.SizeOf(menuItemInfo);

			NativeMethods.InsertMenuItem(_menu, (uint)position, true, ref menuItemInfo);

			return new ContextMenu(submenu);
		}

		public void AppendMenuItem(string text, int id, Bitmap icon)
		{
			var handleToIcon = _ResizeBitmap(icon, _GetRequiredSize()).GetHbitmap();
			var menuItemInfo = new MENUITEMINFO
			{
				fType = MFT.MFT_BITMAP | MFT.MFT_STRING,
				fMask = MIIM.MIIM_BITMAP | MIIM.MIIM_STRING | MIIM.MIIM_ID,
				hbmpItem = handleToIcon,
				dwTypeData = text,
				cch = (uint)text.Length,
				wID = (uint)id
			};
			menuItemInfo.cbSize = (uint)Marshal.SizeOf(menuItemInfo);

			NativeMethods.InsertMenuItem(_menu, (uint)_itemsAdded, true, ref menuItemInfo);
			_itemsAdded++;
		}

		private Size _GetRequiredSize()
		{
			//TODO: read this from the system
			return new Size(16, 16);
		}

		private Bitmap _ResizeBitmap(Bitmap icon, Size desiredSize)
		{
			return new Bitmap(icon, desiredSize);
		}
	}
}
