using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DrawShip.Viewer
{
	public class HostingContext
	{
		private static HostingContext _instance;

		private readonly ApplicationContext _applicationContext;
		private readonly IOwinHost _host;
		private readonly IDictionary<Guid, string> _directoryKeys = new Dictionary<Guid, string>();

		public HostingContext(ApplicationContext applicationContext, IOwinHost host)
		{
			if (_instance != null)
				throw new InvalidOperationException("Not allowed to have multiple instances");

			_applicationContext = applicationContext;
			_host = host;
			_instance = this;

			_directoryKeys.Add(Guid.NewGuid(), applicationContext.WorkingDirectory);
		}

		public static HostingContext Instance
		{
			[DebuggerStepThrough]
			get { return _instance; }
		}

		public int Port
		{
			[DebuggerStepThrough]
			get { return _host.Port; }
		}

		public ApplicationContext ApplicationContext
		{
			[DebuggerStepThrough]
			get { return _applicationContext; }
		}

		public void ApplicationStarted()
		{
			DisplayDrawing(
				new ShowDiagramStructure
				{
					FileName = _applicationContext.FileName,
					Directory = _applicationContext.WorkingDirectory,
					Format = _applicationContext.Format
				});
		}

		public string GetDirectory(string directoryKey)
		{
			var key = Guid.Parse(directoryKey);

			return _directoryKeys.ContainsKey(key)
				? _directoryKeys[key]
				: null;
		}

		public void DisplayDrawing(ShowDiagramStructure command)
		{
			var versionQueryString = string.IsNullOrEmpty(command.Version)
				? ""
				: "v=" + command.Version;
			var workingDirectoryKey = _GetWorkingDirectoryKey(command.Directory);
			var formatQueryString = command.Format == DiagramFormat.Html
				? ""
				: "f=" + command.Format;
			var queryString = string.Join("&", new[] { versionQueryString, formatQueryString }.Where(s => !string.IsNullOrEmpty(s)));
			if (!string.IsNullOrEmpty(queryString))
				queryString = "?" + queryString;

			var fileNameAndVersionAndFormat = string.Format("{0}/{1}{2}", workingDirectoryKey, command.FileName, queryString);
			var url = string.Format(
				"http://localhost:{0}/{1}",
				Port,
				fileNameAndVersionAndFormat);

			Process.Start(url);
		}

		private Guid _GetWorkingDirectoryKey(string directory)
		{
			var directoryKeyEntry = _directoryKeys.SingleOrDefault(kvp => kvp.Value.Equals(directory, StringComparison.OrdinalIgnoreCase));

			if (directoryKeyEntry.Key != Guid.Empty)
				return directoryKeyEntry.Key;

			var key = Guid.NewGuid();
			_directoryKeys.Add(key, directory);
			return key;
		}

		public void DisplayIndex()
		{
			var url = string.Format(
				"http://localhost:{0}/",
				Port);

			Process.Start(url);
		}
	}
}
