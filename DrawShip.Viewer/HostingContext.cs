using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DrawShip.Viewer
{
    /// <summary>
    /// The context for the currently running owin host
    /// </summary>
    public class HostingContext
    {
        private static HostingContext _instance;

        private readonly ApplicationContext _applicationContext;
        private readonly Dictionary<int, string> _directoryKeys = new Dictionary<int, string>();
        private readonly int _port;

        public HostingContext(ApplicationContext applicationContext, int port)
        {
            if (_instance != null)
                throw new InvalidOperationException("Not allowed to have multiple instances");

            _port = port;
            _applicationContext = applicationContext;
            _instance = this;

            _directoryKeys.Add(applicationContext.WorkingDirectory.GetHashCode(), applicationContext.WorkingDirectory);
        }

        /// <summary>
        /// A singleton instance for the currently hosted owin application
        /// </summary>
        public static HostingContext Instance
        {
            [DebuggerStepThrough]
            get { return _instance; }
        }

        /// <summary>
        /// The port the owin host is running on
        /// </summary>
        public int Port
        {
            [DebuggerStepThrough]
            get { return _port; }
        }

        /// <summary>
        /// The process/application context the hosted owin application is running within
        /// </summary>
        public ApplicationContext ApplicationContext
        {
            [DebuggerStepThrough]
            get { return _applicationContext; }
        }

        /// <summary>
        /// The owin host has finished starting, trigger any relevant events
        /// </summary>
        public void OnApplicationStarted()
        {
            ExecuteCommand(
                new ShowDiagramStructure
                {
                    FileName = _applicationContext.FileName,
                    Directory = _applicationContext.WorkingDirectory,
                    Format = _applicationContext.Format
                });
        }

        /// <summary>
        /// Get the directory for a given directory key (which will map to a physical path)
        /// </summary>
        /// <param name="directoryKey"></param>
        /// <returns></returns>
        public string GetDirectory(int directoryKey)
        {
            return _directoryKeys.ContainsKey(directoryKey)
                ? _directoryKeys[directoryKey]
                : null;
        }

        /// <summary>
        /// Execute the given command
        /// If the command is to print a drawing, then the drawing will be printed directly (without rendering the drawing in a browser)
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(ShowDiagramStructure command)
        {
            if (command.Format == DiagramFormat.Print)
                _applicationContext.PrintDrawing(command);

            _OpenDrawing(command);
        }

        /// <summary>
        /// Open the drawing in a browser, formatting the url for the command appropriately to maintain all the parameters
        /// </summary>
        /// <param name="command"></param>
        private void _OpenDrawing(ShowDiagramStructure command)
        {
            var workingDirectoryKey = _GetWorkingDirectoryKey(command.Directory);

            var url = WebApiStartup.FormatUrl(workingDirectoryKey, command.FileName, command.Format, command.Version);
            Process.Start(url.ToString());
        }

        /// <summary>
        /// Get a directory key for the given physical path, if one cannot be found - create one and return it
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private int _GetWorkingDirectoryKey(string directory)
        {
            var directoryKeyEntry = _directoryKeys.SingleOrDefault(kvp => kvp.Value.Equals(directory, StringComparison.OrdinalIgnoreCase));

            if (directoryKeyEntry.Key != 0)
                return directoryKeyEntry.Key;

            var key = directory.GetHashCode();
            _directoryKeys.Add(key, directory);
            return key;
        }

        /// <summary>
        /// Open the index page in a browser, this should present all of the registered paths for the current hosting context
        /// </summary>
        public void DisplayIndex()
        {
            var url = string.Format(
                "http://localhost:{0}/",
                Port);

            Process.Start(url);
        }

        public IReadOnlyDictionary<int, string> GetDirectoryPaths()
        {
            return _directoryKeys;
        }
    }
}
