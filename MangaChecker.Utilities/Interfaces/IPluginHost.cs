using System;
using System.Collections.Generic;
using MangaChecker.Data.Interfaces;

namespace MangaChecker.Utilities.Interfaces {
    public interface IPluginHost {
        IEnumerable<Lazy<IPlugin, IPluginMetadata>> Plugins { get; set; }
        void Dispose();
        void Initialize();
    }
}