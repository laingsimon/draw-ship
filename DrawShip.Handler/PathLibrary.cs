using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DrawShip.Handler
{
	public class PathLibrary
	{
		private readonly IReadOnlyDictionary<string, string> _lookup;
		private readonly string _defaultPath;

		public PathLibrary(IReadOnlyDictionary<string, string> lookup, string defaultPath)
		{
			if (string.IsNullOrEmpty(defaultPath))
				throw new ArgumentNullException(nameof(defaultPath));

			_lookup = lookup;
			_defaultPath = defaultPath;
		}

		public PathLibrary()
			: this(_ReadLookup(), _ReadDefaultPath())
		{ }

		private static string _ReadDefaultPath()
		{
			return ConfigurationManager.AppSettings["defaultLibraryPath"];
		}

		private static IReadOnlyDictionary<string, string> _ReadLookup()
		{
			const string prefix = "library:";
			var libraryKeys = ConfigurationManager.AppSettings.AllKeys.Where(key => key.StartsWith(prefix));
			return (from key in libraryKeys
				   let alias = key.Substring(prefix.Length)
				   let path = ConfigurationManager.AppSettings[key]
				   select new { alias, path })
				   .ToDictionary(a => a.alias, a => a.path, StringComparer.OrdinalIgnoreCase);
		}

		public string GetPhysicalPath(string library)
		{
			if (string.IsNullOrEmpty(library))
				return _defaultPath;

			if (_lookup.ContainsKey(library))
				return _lookup[library];

			return null;
		}
	}
}
