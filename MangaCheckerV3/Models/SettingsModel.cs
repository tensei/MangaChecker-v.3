using System;
using MangaChecker.DataTypes.Interfaces;
using PropertyChanged;

namespace MangaCheckerV3.Models {
    [ImplementPropertyChanged]
    public class SettingsModel {
        private readonly IPluginMetadata _metadata;
        private readonly IPlugin _settings;

        public SettingsModel(Lazy<IPlugin, IPluginMetadata> settings) {
            _settings = settings.Value;
            _metadata = settings.Metadata;
        }

        public object SettingsView => _settings.SettingsView();
        public string Name => _metadata.Title;
        public string Author => _metadata.Author;
        public string Version => _metadata.Version;
        public string Description => _metadata.Description;
    }
}