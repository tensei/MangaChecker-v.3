using System.Collections.ObjectModel;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class PluginsViewModel {
        private readonly ObservableCollection<object> _plugins = new ObservableCollection<object>();

        public PluginsViewModel() {
            Plugins = new ReadOnlyObservableCollection<object>(_plugins);
            foreach (var instanceSetting in PluginHost.Instance.Settings)
                _plugins.Add(new SettingsModel(instanceSetting));
        }

        public ReadOnlyObservableCollection<object> Plugins { get; }
    }
}