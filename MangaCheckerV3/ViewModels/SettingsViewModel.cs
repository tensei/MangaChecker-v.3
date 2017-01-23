using System.Collections.ObjectModel;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class SettingsViewModel {
		public static SettingsViewModel Instance;

		private readonly ObservableCollection<object> _siteSetting = new ObservableCollection<object>();

        public SettingsViewModel() {
			Instance = this;
			Sitesettings = new ReadOnlyObservableCollection<object>(_siteSetting);

			foreach (var instanceSite in PluginHost.Instance.Sites) {
				if (instanceSite.Value.SettingsView() == null) continue;
				_siteSetting.Add(new SiteModel(instanceSite));
			}
			foreach (var instanceSetting in PluginHost.Instance.Settings) _siteSetting.Add(new SettingsModel(instanceSetting));
		}

		public ReadOnlyObservableCollection<object> Sitesettings { get; }
		public object SelectedSetting { get; set; }
	}
}