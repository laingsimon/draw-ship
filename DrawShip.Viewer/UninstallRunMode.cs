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
            _DynamicVerbItems();
            _StaticVerbItems();
            return true;
        }

        private void _DynamicVerbItems()
        {
            RegAsm.Execute("/unregister");
        }

        private void _StaticVerbItems()
        {
            _UninstallStaticVerbItems(".xml");
            _UninstallStaticVerbItems(".drawio");
        }

        private void _UninstallStaticVerbItems(string extension)
        {
            var xml = Registry.ClassesRoot.OpenKey(extension, createIfRequired: true);
            var xmlFileType = (string)xml.GetValue(null, null);
            if (xmlFileType == null)
                return;

            var windows10ExtensionShell = Registry.ClassesRoot.OpenPath($@"SystemFileAssociations\{extension}\shell");
            var extensionShell = Registry.ClassesRoot.OpenPath(xmlFileType + @"\shell");

            _RemoveContextMenuItems(
                new[] { windows10ExtensionShell, extensionShell },
                new[] { InstallRunMode.HtmlPreviewContextMenuName, InstallRunMode.ImagePreviewContextMenuName, InstallRunMode.PrintContextMenuName });
        }

        private void _RemoveContextMenuItems(RegistryKey[] registryKey, string[] itemNames)
        {
            foreach (var key in registryKey.Where(k => k != null))
                foreach (var name in itemNames)
                    key.DeleteSubKeyTree(name, false);
        }
    }
}
