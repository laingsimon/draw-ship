using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DrawShip.Handler
{
	/// <summary>
	/// A library of paths statically configured (in the web.config) which facilitate multiple paths for the IIS http handler
	/// HTTP requests will indicate the library to use, which maps to a physical path, if no library is specified in the
	/// HTTP request, the default library/path will be used.
	/// </summary>
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

		/// <summary>
		/// Given the name of the library, from the HTTP request, get the path to the drawings
		/// </summary>
		/// <param name="library"></param>
		/// <returns></returns>
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
