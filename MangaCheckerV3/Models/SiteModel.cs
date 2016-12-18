using System;
using MangaChecker.Interfaces;

namespace MangaCheckerV3.Models {
	public class SiteModel {
		private readonly IPluginMetadata _metadata;
		private readonly ISite _site;

		public SiteModel(Lazy<ISite, IPluginMetadata> site) {
			_site = site.Value;
			_metadata = site.Metadata;
		}

		public object View => _site.View();
		public object SettingsView => _site.SettingsView();
		public object AddView => _site.AddView();

		public string Name => _metadata.Title;
		public string Author => _metadata.Author;
		public string Version => _metadata.Version;
		public string Description => _metadata.Description;
	}
}