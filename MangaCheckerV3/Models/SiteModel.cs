using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MangaChecker.Interfaces;

namespace MangaCheckerV3.Models {
	public class SiteModel {
		private readonly ISite _site;

		private readonly IPluginMetadata _metaData;
		public string Name => _site.Name();
		public object View => _site.View();
		public object SettingsView => _site.SettingsView();
		public object AddView => _site.AddView();

		public string Author {
			get { return _metaData.Author; }
		}

		public string Description => _metaData.Description;
		public string Version => _metaData.Version;

		public SiteModel(Lazy<ISite, IPluginMetadata> site) {
			_site = site.Value;
			_metaData = site.Metadata;
		}
	}
}
