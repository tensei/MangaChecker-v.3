using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MangaChecker.Interfaces;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class MangaListViewModel {
		/// <summary>
		///     Initializes a new instance of the MangaListViewModel class.
		/// </summary>
		private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

		private static readonly Dictionary<string, string> _listboxItemNames = new Dictionary<string, string> {
			{"All", null},
			{"Mangareader", null},
			{"Mangafox", null},
			{"Mangahere", null},
			{"Mangastream", null},
			{"Batoto", null},
			{"Kissmanga", null},
			{"Webtoons", null},
			{"Yomanga", null},
			{"GoScanlation", "GameOfScanlation"},
			{"KireiCake", null},
			{"Jaiminisbox", null},
			{"HeyManga", null},
			{"Backlog", null},
			{"DEBUG", null}
		};

		public static MangaListViewModel Instance;
		public MangaListViewModel() {
			Instance = this;
			Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
		}

		public ReadOnlyObservableCollection<Manga> Mangas { get; }

		public Manga SelectedManga { get; set; }
		public List<SiteModel> Sites => FillSites();

		public SiteModel SelectedSite { get; set; }

		private List<SiteModel> FillSites() {
			return new List<SiteModel>(PluginHost.Instance.Sites.Select(s=> new SiteModel(s)));
		}
	}
}