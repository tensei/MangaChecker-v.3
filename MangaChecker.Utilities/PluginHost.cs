using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using MangaChecker.Data.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Utilities {
    public class PluginHost : IDisposable, IPluginHost {
        private const string PluginsDirectory = "Plugins";
        private readonly CompositionContainer _container;

        public PluginHost() {
            var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            //var current = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            //if (current == null) {
            //    return;
            //}
            var pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), PluginsDirectory);
            if (!Directory.Exists(pluginsPath)) {
                return;
            }
            var folders = Directory.GetDirectories(pluginsPath);
            foreach (var folder in folders) {
                catalog.Catalogs.Add(new DirectoryCatalog(folder));
            }
            _container = new CompositionContainer(catalog);
            Initialize();
        }

        public void Dispose() {
            foreach (var plugin in Plugins) {
                try {
                    plugin?.Value?.Dispose();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            _container?.Dispose();
        }


        [ImportMany]
        public IEnumerable<Lazy<IPlugin, IPluginMetadata>> Plugins { get; set; }

        public void Initialize() {
            _container.ComposeParts(this);
            foreach (var plugin in Plugins) {
                plugin.Value.Initialize();
            }
        }
    }
}