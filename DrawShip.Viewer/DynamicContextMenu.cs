using DrawShip.Viewer.ComInterop;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace DrawShip.Viewer
{
	[ComVisible(true), Guid(_comGuid)]
	public class DynamicContextMenu : IContextMenu, IShellExtInit
	{
		private const string _comGuid = "7275228C-56F1-4BF8-A1B7-3E660F3930A7";
		private const string _xmlPreviousProgNameValueName = "DrawShipInstall_PrevProgName";
		private const int GCS_VERBW = 4;

		private const int _htmlVerb = 0;
		private const int _imageVerb = 1;
		private const int _printVerb = 2;
		private string _fileName;

		#region (un)installation
		[ComRegisterFunction]
		public static void Register(string anything)
		{
			var clsid = "{" + typeof(DynamicContextMenu).GUID + "}";
			var progId = typeof(DynamicContextMenu).FullName;

			var contextMenuHandler = Registry.ClassesRoot.OpenPath(progId + @"\shellex\ContextMenuHandlers\DrawShip", createIfRequired: true);
			contextMenuHandler.SetValue(null, clsid);
			var progName = Registry.ClassesRoot.OpenKey(progId);
			progName.SetValue(null, "DrawShip");

			var xml = Registry.ClassesRoot.OpenKey(@".xml", createIfRequired: true);
			var prevProgram = (string)xml.GetValue(null, null);
			if (prevProgram != null && prevProgram != progId)
				xml.SetValue(_xmlPreviousProgNameValueName, prevProgram);
			xml.SetValue(null, progId);

            var xmlProg = Registry.ClassesRoot.OpenKey(@"CLSID\" + clsid);
			if (xmlProg == null)
				return; //not installed properly?

			xmlProg.OpenKey(@"shellex\MayChangeDefaultMenu", createIfRequired: true);

		    var allContextMenuHandlers = Registry.ClassesRoot.OpenKey("*\\shellex\\ContextMenuHandlers\\DrawShip",
		        createIfRequired: true);
            allContextMenuHandlers.SetValue(null, clsid);
		}

		[ComUnregisterFunction]
		public static void UnRegister(string anything)
		{
			var xml = Registry.ClassesRoot.OpenKey(@".xml", createIfRequired: true);
			var prevProgram = (string)xml.GetValue(_xmlPreviousProgNameValueName, null);
			if (prevProgram != null)
				xml.SetValue(null, prevProgram);
			xml.DeleteValue(_xmlPreviousProgNameValueName);

            var allContextMenuHandlers = Registry.ClassesRoot.OpenKey("*\\shellex\\ContextMenuHandlers");
            allContextMenuHandlers.DeleteSubKey("DrawShip");
        }
		#endregion

		void IContextMenu.GetCommandString(int idcmd, uint uflags, int reserved, StringBuilder commandstring, int cch)
		{
			if (uflags != GCS_VERBW)
				return;

			if (idcmd == _htmlVerb)
				commandstring.Append(_MaxLength("DrawShip.Preview-Html", cch));
			else if (idcmd == _imageVerb)
				commandstring.Append(_MaxLength("DrawShip.Preview-Image", cch));
			else if (idcmd == _printVerb)
				commandstring.Append(_MaxLength("DrawShip.Print-Drawing", cch));
		}

		private static string _MaxLength(string value, int maxLength)
		{
			if (value.Length > maxLength)
				return value.Substring(0, maxLength);

			return value;
		}

		int IShellExtInit.Initialize(IntPtr pidlFolder, IntPtr lpdobj, uint hKeyProgID)
		{
			var dataObject = (IDataObject)Marshal.GetObjectForIUnknown(lpdobj);

			var format = new FORMATETC
			{
				cfFormat = (short)CLIPFORMAT.CF_HDROP,
				dwAspect = DVASPECT.DVASPECT_CONTENT,
				lindex = -1,
				ptd = IntPtr.Zero,
				tymed = TYMED.TYMED_HGLOBAL
			};
			var medium = new STGMEDIUM();

			dataObject.GetData(ref format, out medium);

			_fileName = null;
			var hGlobal = medium.unionmember;
			var noOfFiles = NativeMethods.DragQueryFile(hGlobal, 0xffffffff, null, 0);

			if (noOfFiles != 1)
				return 0;

			StringBuilder sb = new StringBuilder(1024);
			NativeMethods.DragQueryFile(hGlobal, 0, sb, sb.Capacity + 1);

			_fileName = sb.ToString();

			return 0;
		}

		private static int? _GetVerb(IntPtr pici)
		{
			var ici = Marshal.PtrToStructure<CMINVOKECOMMANDINFO>(pici);

			var iciex = new CMINVOKECOMMANDINFOEX();

			var isUnicode = false;
			if (ici.cbSize == Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX)))
			{
				if ((ici.fMask & CMIC.CMIC_MASK_UNICODE) != 0)
				{
					isUnicode = true;
					iciex = (CMINVOKECOMMANDINFOEX)Marshal.PtrToStructure(pici,
						typeof(CMINVOKECOMMANDINFOEX));
				}
			}

			// For the ANSI case, if the high-order word is not zero, the command's
			// verb string is in lpcmi->lpVerb.
			if (!isUnicode && NativeMethods.HighWord(ici.verb.ToInt32()) != 0)
			{
				// Is the verb supported by this context menu extension?
				var verb = Marshal.PtrToStringAnsi(ici.verb);
				if (verb == "Html" || verb == "Image" || verb == "Print")
				{
					return verb.GetHashCode();
				}
				else
				{
					// If the verb is not recognized by the context menu handler, it
					// must return E_FAIL to allow it to be passed on to the other
					// context menu handlers that might implement that verb.
					return null;
				}
			}
			// For the Unicode case, if the high-order word is not zero, the
			// command's verb string is in lpcmi->lpVerbW.
			else if (isUnicode && NativeMethods.HighWord(iciex.verbW.ToInt32()) != 0)
			{
				// Is the verb supported by this context menu extension?
				var verb = Marshal.PtrToStringUni(iciex.verbW);
				if (verb == "Html" || verb == "Print" || verb == "Image")
				{
					return verb.GetHashCode();
				}
				else
				{
					// If the verb is not recognized by the context menu handler, it
					// must return E_FAIL to allow it to be passed on to the other
					// context menu handlers that might implement that verb.
					return null;
				}
			}

			// If the command cannot be identified through the verb string, then
			// check the identifier offset.
			else
			{
				// Is the command identifier offset supported by this context menu
				// extension?
				var verb = NativeMethods.LowWord(ici.verb.ToInt32());
				if (verb >= 0 && verb <= _printVerb)
				{
					return verb;
				}
				else
				{
					// If the verb is not recognized by the dcontext menu handler, it
					// must return E_FAIL to allow it to be passed on to the other
					// context menu handlers that might implement that verb.
					return null;
				}
			}
		}

		void IContextMenu.InvokeCommand(IntPtr pici)
		{
			try
			{
				var verb = _GetVerb(pici);
				if (verb == null)
					Marshal.ThrowExceptionForHR(WinError.E_FAIL);

				switch (verb.Value)
				{
					case _htmlVerb:
						_StartDrawIo(_fileName, DiagramFormat.Html);
						break;
					case _imageVerb:
						_StartDrawIo(_fileName, DiagramFormat.Image);
						break;
					case _printVerb:
						_StartDrawIo(_fileName, DiagramFormat.Print);
						break;
					default:
						return;
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.ToString(), "DrawShip.Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void _StartDrawIo(string fileName, DiagramFormat format)
		{
			Process.Start(new ProcessStartInfo
			{
				Arguments = string.Format("\"{0}\" /format:{1}", fileName, format),
				WorkingDirectory = Path.GetDirectoryName(fileName),
				FileName = this.GetType().Assembly.Location
			});
		}

		int IContextMenu.QueryContextMenu(IntPtr hMenu, int iMenu, int idCmdFirst, int idCmdLast, CMF uFlags)
		{
			if (string.IsNullOrEmpty(_fileName))
				return 0;

			var extension = Path.GetExtension(_fileName);
			if (string.IsNullOrEmpty(extension) || !extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
				return 0;

			if (!_IsDrawing(_fileName))
				return 0;

			if (uFlags.HasFlag(CMF.CMF_DEFAULTONLY))
				return 0;

			var code = (uint)(idCmdFirst + _printVerb + 1);
			var menu = new ContextMenu(hMenu);

			var submenu = menu.CreateSubMenu("Draw Ship", 4, Properties.Resources.tray_icon.ToBitmap());
			submenu.AppendMenuItem("Html Viewer", idCmdFirst + _htmlVerb, Properties.Resources.PreviewHtml);
			submenu.AppendMenuItem("Image Viewer", idCmdFirst + _imageVerb, Properties.Resources.PreviewImage);
			submenu.AppendMenuItem("Print", idCmdFirst + _printVerb, Properties.Resources.PrintDrawing);

			return WinError.MAKE_HRESULT(0, 0, 3);
		}

		private bool _IsDrawing(string fileName)
		{
			using (var reader = new StreamReader(fileName))
			{
				var buffer = new char[10];
				var length = reader.ReadBlock(buffer, 0, buffer.Length);

				var startingOfFile = new String(buffer, 0, length);
				return startingOfFile.StartsWith("<mxfile ");
			}
		}
	}
}
