using System;
using System.Collections.Generic;

namespace DrawShip.Viewer
{
	public class IndexViewModel
	{
		private readonly HostingContext _hostingContext;

		public IndexViewModel(HostingContext hostingContext)
		{
			_hostingContext = hostingContext;
		}

		public IReadOnlyDictionary<int, string> Paths => _hostingContext.GetDirectoryPaths();
		public string ExecutingPath => GetType().Assembly.Location;
	}
}
