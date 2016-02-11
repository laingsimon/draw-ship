using Microsoft.Win32;
using System;

namespace DrawShip.Viewer
{
	public static class RegistryExtensions
	{
		public static RegistryKey OpenKey(this RegistryKey key, string name, bool createIfRequired = false)
		{
			if (key == null)
				return null;

			var existingSubKey = key.OpenSubKey(name, true);
			if (existingSubKey != null || !createIfRequired)
				return existingSubKey;

			return key.CreateSubKey(name);
		}

		public static RegistryKey OpenPath(this RegistryKey key, string path, bool createIfRequired = false)
		{
			var pathSegments = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var segment in pathSegments)
			{
				key = key.OpenKey(segment, createIfRequired: createIfRequired);

				if (key == null)
					return null;
			}

			return key;
		}
	}
}
