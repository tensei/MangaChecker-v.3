using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Interfaces;
using MangaCheckerV3.Models;

namespace MangaCheckerV3.Common {
	public class PluginHost : IDisposable {

		public const string PluginsDirectory = "Plugins";

		private static readonly PluginHost instance = new PluginHost();
		
		public static PluginHost Instance {
			get { return instance; }
		}


		private readonly CompositionContainer container;

		[ImportMany]
		public IEnumerable<Lazy<ISite, IPluginMetadata>> Sites { get; set; }
		[ImportMany]
		public IEnumerable<Lazy<ISettingsPlugin, IPluginMetadata>> Settings { get; set; }


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

		static PluginHost() {

		}
		public void Dispose() {
			container.Dispose();
		}
		
		public void Initialize() {
			container.ComposeParts(this);
			GetSites().Initialize();
		}

		private ISite GetSites() {
			return new AggregateSites(Sites.Select(s=>s.Value));
		}
	}
}
