using Microsoft.Win32;
using System.Linq;

namespace DrawShip.Viewer
{
	/// <summary>
	/// Run mode for uninstalling the context menu items from windows
	/// </summary>
	public class UninstallRunMode : IRunMode
	{
		public bool Run(ApplicationContext applicationContext)
		{
			var xml = Registry.ClassesRoot.OpenKey(@".xml", createIfRequired: true);
			var xmlFileType = (string)xml.GetValue(null, null);
			if (xmlFileType == null)
			{
				xmlFileType = "xmlfile";
				xml.SetValue(null, xmlFileType);
			}

			var windows10XmlShell = Registry.ClassesRoot.OpenPath(@"SystemFileAssociations\.xml\shell");
			var xmlShell = Registry.ClassesRoot.OpenPath(xmlFileType + @"\shell");

			_RemoveContextMenuItems(
				new[] { windows10XmlShell, xmlShell },
				new[] { InstallRunMode.HtmlPreviewContextMenuName, InstallRunMode.ImagePreviewContextMenuName, InstallRunMode.PrintContextMenuName });
			
			return true;
		}

		private void _RemoveContextMenuItems(RegistryKey[] registryKey, string[] itemNames)
		{
			foreach (var key in registryKey.Where(k => k != null))
			foreach (var name in itemNames)
				key.DeleteSubKeyTree(name, false);
		}
	}
}
