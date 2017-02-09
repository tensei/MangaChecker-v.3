using System.Collections.ObjectModel;
using MangaChecker.Data.Model;
using MangaChecker.Utilities;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class PluginsViewModel {
        private readonly ObservableCollection<PluginModel> _plugins = new ObservableCollection<PluginModel>();

        public PluginsViewModel() {
            Plugins = new ReadOnlyObservableCollection<PluginModel>(_plugins);
            foreach (var instanceSetting in PluginHost.Instance.Plugins) {
                _plugins.Add(new PluginModel(instanceSetting));
            }
        }

        public ReadOnlyObservableCollection<PluginModel> Plugins { get; }
    }
}