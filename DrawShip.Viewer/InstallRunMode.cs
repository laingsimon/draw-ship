using Microsoft.Win32;
using System;
using System.Linq;

namespace DrawShip.Viewer
{
	public class InstallRunMode : IRunMode
	{
		public const string HtmlPreviewContextMenuName = "Preview in DrawShip";
		public const string ImagePreviewContextMenuName = "Preview in DrawShip (image)";

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
			_CreateHtmlPreviewItem(xmlFileShell);
			_CreateImagePreviewItem(xmlFileShell);

			return true;
		}

		private static void _CreateHtmlPreviewItem(RegistryKey xmlFileShell)
		{
			var preview = xmlFileShell.OpenSubKey(HtmlPreviewContextMenuName, true) ?? xmlFileShell.CreateSubKey(HtmlPreviewContextMenuName);
			var previewCommand = preview.OpenSubKey("command", true) ?? preview.CreateSubKey("command");

			var applicationExePath = Environment.GetCommandLineArgs().First();
			previewCommand.SetValue(null, string.Format("\"{0}\" \"%1\"", applicationExePath));
		}

		private static void _CreateImagePreviewItem(RegistryKey xmlFileShell)
		{
			var preview = xmlFileShell.OpenSubKey(ImagePreviewContextMenuName, true) ?? xmlFileShell.CreateSubKey(ImagePreviewContextMenuName);
			var previewCommand = preview.OpenSubKey("command", true) ?? preview.CreateSubKey("command");

			var applicationExePath = Environment.GetCommandLineArgs().First();
			previewCommand.SetValue(null, string.Format("\"{0}\" \"%1\" /format:Image", applicationExePath));
		}
	}
}
