using DrawShip.Viewer.ComInterop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DrawShip.Viewer
{
    public class ContextMenu
    {
        private const int SM_CXSMICON = 49;
        private const int SM_CYSMICON = 50;

        private static readonly Lazy<Size> _getIconSize = new Lazy<Size>(_GetRequiredSize);
        private readonly IntPtr _menu;
        private int _itemsAdded = 0;

        public ContextMenu(IntPtr menu)
        {
            _menu = menu;
        }

        public ContextMenu CreateSubMenu(string text, int position, Image icon)
        {
            var submenu = NativeMethods.CreatePopupMenu();
            var handleToIcon = _ResizeIcon(icon, _getIconSize.Value);
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

        public void AppendMenuItem(string text, int id, Image icon)
        {
            var handleToIcon = _ResizeIcon(icon, _getIconSize.Value);
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

        private static Size _GetRequiredSize()
        {
            var width = NativeMethods.GetSystemMetrics(SM_CXSMICON);
            var height = NativeMethods.GetSystemMetrics(SM_CYSMICON);

            return new Size(width, height);
        }

        private IntPtr _CreateTransparentIcon(Bitmap icon)
        {
            IntPtr hBitmap, ppvBits;
            BITMAPINFO bmi = new BITMAPINFO();
            bmi.biSize = 40;            // Needed for RtlMoveMemory()
            bmi.biBitCount = 32;        // Number of bits
            bmi.biPlanes = 1;           // Number of planes
            bmi.biWidth = icon.Width;     // Width of our new bitmap
            bmi.biHeight = icon.Height;   // Height of our new bitmap
            icon.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // Required due to the way bitmap is copied and read

            hBitmap = NativeMethods.CreateDIBSection(new IntPtr(0), bmi, 0,
                      out ppvBits, new IntPtr(0), 0);
            //create our new bitmap
            BitmapData bitmapData = icon.LockBits(new Rectangle(0, 0,
                       icon.Width, icon.Height), ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);
            NativeMethods.RtlMoveMemory(ppvBits, bitmapData.Scan0,
                           icon.Height * bitmapData.Stride);
            // copies the bitmap
            icon.UnlockBits(bitmapData);

            return hBitmap;
        }

        private IntPtr _ResizeIcon(Image icon, Size desiredSize)
        {
            var bitmap = icon as Bitmap;

            if (icon.Size == desiredSize && bitmap != null)
                return _CreateTransparentIcon(bitmap);

            var resizedIcon = new Bitmap(icon, desiredSize);
            return _CreateTransparentIcon(resizedIcon);
        }
    }
}
