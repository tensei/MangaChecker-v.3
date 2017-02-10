using System.Collections.ObjectModel;
using MangaChecker.Data.Models;
using MangaChecker.Utilities.Interfaces;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels {
    [ImplementPropertyChanged]
    public class PluginsViewModel {
        private readonly ObservableCollection<PluginModel> _plugins = new ObservableCollection<PluginModel>();

        public PluginsViewModel(IPluginHost pluginHost) {
            Plugins = new ReadOnlyObservableCollection<PluginModel>(_plugins);
            foreach (var instanceSetting in pluginHost.Plugins) {
                _plugins.Add(new PluginModel(instanceSetting));
            }
        }

        public ReadOnlyObservableCollection<PluginModel> Plugins { get; }
    }
}