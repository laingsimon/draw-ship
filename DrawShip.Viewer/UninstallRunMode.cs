using Microsoft.Win32;
using System;
using System.Linq;

namespace DrawShip.Viewer
{
	public class UninstallRunMode : IRunMode
	{
		public bool Run(ApplicationContext applicationContext)
		{
			var xml = Registry.ClassesRoot.OpenSubKey(".xml") ?? Registry.ClassesRoot.CreateSubKey(".xml");
			var xmlFileType = (string)xml.GetValue(null, null);
			if (xmlFileType == null)
			{
				xmlFileType = "xmlfile";
				xml.SetValue(null, xmlFileType);
			}

			var xmlFileNode = Registry.ClassesRoot.OpenSubKey(xmlFileType);
			var xmlFileShell = xmlFileNode.OpenSubKey("shell", true) ?? xmlFileNode.CreateSubKey("shell");
			xmlFileShell.DeleteSubKeyTree(InstallRunMode.ContextMenuName, false);

			return true;
		}
	}
}
