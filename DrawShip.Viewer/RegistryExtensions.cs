using Microsoft.Win32;
using System;

namespace DrawShip.Viewer
{
    /// <summary>
    /// Helper methods for working with the registry
    /// </summary>
    public static class RegistryExtensions
    {
        /// <summary>
        /// Open a registry key with the given name under the given key, creating it if it cannot be found
        /// The key will be opened in writeable mode
        /// </summary>
        /// <param name="key">The parent key</param>
        /// <param name="name">The name of the child key</param>
        /// <param name="createIfRequired">Should the key be created if it cannot be found</param>
        /// <returns></returns>
        public static RegistryKey OpenKey(this RegistryKey key, string name, bool createIfRequired = false)
        {
            if (key == null)
                return null;

            var existingSubKey = key.OpenSubKey(name, true);
            if (existingSubKey != null || !createIfRequired)
                return existingSubKey;

            return key.CreateSubKey(name);
        }

        /// <summary>
        /// Open a registry key under the current key, and at the given relative path, creating it if it cannot be found
        /// Key is opened in writeable mode
        /// </summary>
        /// <param name="key">The root key</param>
        /// <param name="path">A relative path to a key under the given root</param>
        /// <param name="createIfRequired">Should the key be created if it cannot be found</param>
        /// <returns></returns>
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
