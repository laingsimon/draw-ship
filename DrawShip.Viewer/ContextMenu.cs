using DrawShip.Viewer.ComInterop;
using System;

namespace DrawShip.Viewer
{
	public class ContextMenu
	{
		private readonly IntPtr _menu;
		private readonly int _positionOffset;

		public ContextMenu(IntPtr menu)
		{
			_menu = menu;
		}

		public ContextMenu CreateSubMenu(string text, int position)
		{
			var submenu = NativeMethods.CreatePopupMenu();
			NativeMethods.InsertMenu(_menu, position, MFMENU.MF_BYPOSITION | MFMENU.MF_POPUP | MFMENU.MF_ENABLED, submenu, text);

			return new ContextMenu(submenu);
		}

		public void AppendMenuItem(string text, int id)
		{
			NativeMethods.AppendMenu(_menu, MFMENU.MF_STRING, new IntPtr(id), text);
		}
	}
}
