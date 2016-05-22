using DrawShip.Handler.ComInterop;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DrawShip.Viewer
{
	[ComVisible(true), Guid(_comGuid)]
	public class DynamicContextMenu
	{
		private const string _comGuid = "7275228C-56F1-4BF8-A1B7-3E660F3930A7";
		private const string _xmlPreviousProgNameValueName = "DrawShipInstall_PrevProgName";

		#region (un)installation
		[ComRegisterFunction]
		public static void Register(string anything)
		{
			var applicationExePath = Environment.GetCommandLineArgs().First();
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

			var shellex = xmlProg.OpenKey(@"shellex\MayChangeDefaultMenu", createIfRequired: true);
		}

		[ComUnregisterFunction]
		public static void UnRegister(string anything)
		{
			var xml = Registry.ClassesRoot.OpenKey(@".xml", createIfRequired: true);
			var prevProgram = (string)xml.GetValue(_xmlPreviousProgNameValueName, null);
			if (prevProgram != null)
				xml.SetValue(null, prevProgram);
			xml.DeleteValue(_xmlPreviousProgNameValueName);
		}
		#endregion
	}
}
