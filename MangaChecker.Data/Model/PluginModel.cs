using System;
using MangaChecker.Data.Interface;
using PropertyChanged;

namespace MangaChecker.Data.Model {
    [ImplementPropertyChanged]
    public class PluginModel : IPluginMetadata {
        private readonly IPluginMetadata _metadata;
        private readonly IPlugin _settings;

        public PluginModel(Lazy<IPlugin, IPluginMetadata> settings) {
            _settings = settings.Value;
            _metadata = settings.Metadata;
        }

        public object SettingsView => _settings.SettingsView();
        public string Title => _metadata.Title;
        public string Author => _metadata.Author;
        public string Version => _metadata.Version;
        public string Description => _metadata.Description;
    }
}