using System.Collections.ObjectModel;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class PluginsViewModel {
        private readonly ObservableCollection<PluginsModel> _plugins = new ObservableCollection<PluginsModel>();

        public PluginsViewModel() {
            Plugins = new ReadOnlyObservableCollection<PluginsModel>(_plugins);
            foreach (var instanceSetting in PluginHost.Instance.Settings)
                _plugins.Add(new PluginsModel(instanceSetting));
        }

        public ReadOnlyObservableCollection<PluginsModel> Plugins { get; }
    }
}