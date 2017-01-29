﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using MangaChecker.DataTypes.Interfaces;

namespace MangaCheckerV3.Common {
    public class PluginHost : IDisposable {
        public const string PluginsDirectory = "Plugins";


        private readonly CompositionContainer container;

        static PluginHost() {
        }


        private PluginHost() {
            var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var current = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (current != null) {
                var pluginsPath = Path.Combine(current, PluginsDirectory);
                if (Directory.Exists(pluginsPath)) {
                    var dcat = new DirectoryCatalog(pluginsPath);
                    catalog.Catalogs.Add(dcat);
                }
            }

            container = new CompositionContainer(catalog);
        }

        public static PluginHost Instance { get; } = new PluginHost();

        [ImportMany]
        public IEnumerable<Lazy<IPlugin, IPluginMetadata>> Settings { get; set; }

        public void Dispose() {
            container.Dispose();
        }

        public void Initialize() {
            container.ComposeParts(this);
        }
    }
}