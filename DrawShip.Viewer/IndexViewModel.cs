using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrawShip.Viewer
{
    public class IndexViewModel
    {
        private readonly HostingContext _hostingContext;
        private readonly int? _directoryKey;

        public IndexViewModel(HostingContext hostingContext, int? directoryKey)
        {
            _hostingContext = hostingContext;
            _directoryKey = directoryKey;
        }

        public IReadOnlyDictionary<int, string> Paths => _hostingContext.GetDirectoryPaths();
        public string ExecutingPath => GetType().Assembly.Location;

        public int? DirectoryKey => _directoryKey;

        public IEnumerable<string> Files
        {
            get
            {
                if (_directoryKey != null && !Paths.ContainsKey(_directoryKey.Value))
                    return Enumerable.Empty<string>();

                var path = Paths[_directoryKey.Value];
                return Directory.EnumerateFiles(path, "*.xml").Concat(Directory.EnumerateFiles(path, "*.drawio"))
                    .Select(file => Path.GetFileName(file))
                    .OrderBy(file => Path.GetFileNameWithoutExtension(file));
            }
        }
    }
}
