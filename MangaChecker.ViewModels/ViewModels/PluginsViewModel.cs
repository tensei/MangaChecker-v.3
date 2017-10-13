using System.Collections.ObjectModel;
using System.ComponentModel;
using MangaChecker.Data.Models;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.ViewModels.ViewModels
{
    public class PluginsViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PluginModel> _plugins = new ObservableCollection<PluginModel>();

        public PluginsViewModel(IPluginHost pluginHost)
        {
            Plugins = new ReadOnlyObservableCollection<PluginModel>(_plugins);
            foreach (var instanceSetting in pluginHost.Plugins)
            {
                _plugins.Add(new PluginModel(instanceSetting));
            }
        }

        public ReadOnlyObservableCollection<PluginModel> Plugins { get; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}