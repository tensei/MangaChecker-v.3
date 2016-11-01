using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Interfaces;
using PropertyChanged;

namespace MangaCheckerV3.Models {
	[ImplementPropertyChanged]
	public class SettingsModel {
		private readonly ISettingsPlugin _settings;
		private readonly IPluginMetadata _metadata;
		public object SettingsView => _settings.SettingsView();

		public string Name => _metadata.Title;
		public string Author => _metadata.Author;
		public string Version => _metadata.Version;
		public string Description => _metadata.Description;
		public SettingsModel(Lazy<ISettingsPlugin, IPluginMetadata> settings) {
			_settings = settings.Value;
			_metadata = settings.Metadata;
		}
	}
}
