using System;
using System.Collections.Generic;

namespace DrawShip.Viewer
{
	public class IndexViewModel
	{
		private readonly HostingContext _hostingContext;
		private readonly IReadOnlyDictionary<Guid, string> _paths;

		public IndexViewModel(HostingContext hostingContext)
		{
			_hostingContext = hostingContext;
			_paths = hostingContext.GetDirectoryPaths();
		}

		public IReadOnlyDictionary<Guid, string> Paths => _paths;
		public string ExecutingPath => GetType().Assembly.Location;
	}
}
