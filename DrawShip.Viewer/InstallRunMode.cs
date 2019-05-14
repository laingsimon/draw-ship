using Microsoft.Win32;
using System;
using System.Linq;

namespace DrawShip.Viewer
{
    /// <summary>
    /// Run mode for installing the context menu items into windows to start the application
    /// </summary>
    public class InstallRunMode : IRunMode
    {
        public const string HtmlPreviewContextMenuName = "Preview in DrawShip";
        public const string ImagePreviewContextMenuName = "Preview in DrawShip (image)";
        public const string PrintContextMenuName = "Print using DrawShip";
        private readonly string _applicationExePath;

        public InstallRunMode()
        {
            _applicationExePath = Environment.GetCommandLineArgs().First();
        }

        public bool Run(ApplicationContext applicationContext)
        {
            return _DynamicVerbItems(applicationContext) || _StaticVerbItems();
        }

        private bool _DynamicVerbItems(ApplicationContext applicationContext)
        {
            return RegAsm.Execute("/codebase", "/nologo");
        }

        private bool _StaticVerbItems()
        {
            var xml = Registry.ClassesRoot.OpenKey(@".xml", createIfRequired: true);
            var xmlFileType = (string)xml.GetValue(null, null);
            if (xmlFileType == null)
            {
                xmlFileType = "xmlfile";
                xml.SetValue(null, xmlFileType);
            }

            var windows10XmlShell = Registry.ClassesRoot.OpenPath(@"SystemFileAssociations\.xml\shell", createIfRequired: true);
            var xmlShell = Registry.ClassesRoot.OpenPath(xmlFileType + @"\shell");

            _CreatePreviewItem(HtmlPreviewContextMenuName, "\"%1\"", new[] { xmlShell, windows10XmlShell });
            _CreatePreviewItem(ImagePreviewContextMenuName, "\"%1\" /format:Image", new[] { xmlShell, windows10XmlShell });
            _CreatePreviewItem(PrintContextMenuName, "\"%1\" /format:Print", new[] { xmlShell, windows10XmlShell });

            return true;
        }

        private void _CreatePreviewItem(string name, string commandFormat, RegistryKey[] shells)
        {
            foreach (var shell in shells)
            {
                if (shell == null)
                    continue;

                var preview = shell.OpenKey(name, createIfRequired: true);
                var command = preview.OpenKey("command", createIfRequired: true);
                command.SetValue(null, $@"""{_applicationExePath}"" {commandFormat}");
                preview.SetValue("icon", string.Format("{0},0", _applicationExePath));
            }
        }

        public static bool IsInstalled()
        {
            return DynamicContextMenu.IsRegistered() || _IsRegistered();
        }

        private static bool _IsRegistered()
        {
            var xml = Registry.ClassesRoot.OpenKey(@".xml");
            if (xml == null)
                return false;

            var xmlFileType = (string)xml.GetValue(null, null);
            if (xmlFileType == null)
                return false;

            var shell = Registry.ClassesRoot.OpenPath(xmlFileType + @"\shell");
            var preview = shell.OpenKey(HtmlPreviewContextMenuName);
            var command = preview?.OpenKey("command");
            var value = command?.GetValue(null, null) as string;
            var applicationExePath = Environment.GetCommandLineArgs().First();
            return value == $@"""{applicationExePath}"" ""%1""";
        }
    }
}
