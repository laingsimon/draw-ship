using Microsoft.Win32;
using System;
using System.Linq;

namespace DrawShip.Viewer
{
	public class InstallRunMode : IRunMode
	{
		public const string ContextMenuName = "Preview in DrawShip";

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
			var preview = xmlFileShell.OpenSubKey(ContextMenuName, true) ?? xmlFileShell.CreateSubKey(ContextMenuName);
			var previewCommand = preview.OpenSubKey("command", true) ?? preview.CreateSubKey("command");

			var applicationExePath = Environment.GetCommandLineArgs().First();
			previewCommand.SetValue(null, string.Format("\"{0}\" \"%1\"", applicationExePath));

			return true;
		}
	}
}
