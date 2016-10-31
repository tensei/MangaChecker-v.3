using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MangaChecker.Interfaces;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Models;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.ViewModels.Settings_ViewModels;
using MaterialDesignColors;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class SettingsViewModel {
		public static SettingsViewModel Instance;
		
		private ObservableCollection<SiteModel> _siteSetting = new ObservableCollection<SiteModel>();
		public ReadOnlyObservableCollection<SiteModel> Sitesettings { get; }
		public SiteModel SelectedSetting { get; set; }
		

		public SettingsViewModel() {
			Instance = this;
			Sitesettings= new ReadOnlyObservableCollection<SiteModel>( _siteSetting);
			MangaListViewModel.Instance.Sites.Where(s => s.SettingsView != null).ToList().ForEach(_siteSetting.Add);
		}
	}
}