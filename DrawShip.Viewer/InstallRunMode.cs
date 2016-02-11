﻿using Microsoft.Win32;
using System;
using System.Linq;

namespace DrawShip.Viewer
{
	public class InstallRunMode : IRunMode
	{
		public const string HtmlPreviewContextMenuName = "Preview in DrawShip";
		public const string ImagePreviewContextMenuName = "Preview in DrawShip (image)";
		private readonly string _applicationExePath;

		public InstallRunMode()
		{
			_applicationExePath = Environment.GetCommandLineArgs().First();
		}

		public bool Run(ApplicationContext applicationContext)
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
				command.SetValue(null, string.Format("\"{0}\" {1}", _applicationExePath, commandFormat));
				preview.SetValue("icon", string.Format("{0},0", _applicationExePath));
			}
		}
	}
}
