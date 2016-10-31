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
			IncreaseCommand = new ActionCommand(IncreaseChapter);
			DecreaseCommand = new ActionCommand(DecreaseChapter);
			DeleteCommand = new ActionCommand(DeleteManga);
			OpenMangaCommand = new ActionCommand(OpenMangaSite);
			RefreshCommand = new ActionCommand(RefreshManga);
			ViewerCommand = new ActionCommand(ViewManga);
			DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
		}

		public ReadOnlyObservableCollection<Manga> Mangas { get; }

		public Manga SelectedManga { get; set; }

		public ICommand IncreaseCommand { get; }
		public ICommand DecreaseCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand OpenMangaCommand { get; }
		public ICommand RefreshCommand { get; }
		public ICommand ViewerCommand { get; }
		public ICommand DeselectCommand { get; }

		public List<SiteModel> Sites => FillSites();

		public SiteModel SelectedSite { get; set; }

		public async void Fill() {
			var x = await new Database().GetAllMangas();
			x.ForEach(_mangas.Add);
		}

		private async void IncreaseChapter() {
			SelectedManga.Chapter++;
			SelectedManga.Updated = DateTime.Now;
			await new Database().Update(SelectedManga);
		}

		private async void DecreaseChapter() {
			SelectedManga.Chapter--;
			SelectedManga.Updated = SelectedManga.Updated - TimeSpan.FromDays(1);
			await new Database().Update(SelectedManga);
		}

		private void OpenMangaSite() {
			if (!SelectedManga.Link.Contains("/")) return;
			Process.Start(SelectedManga.Link);
		}

		private void DeleteManga() {
			Task.Run(async () => {
				await new Database().Delete(SelectedManga);
				MainWindowViewModel.Instance.SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO", HandleUndoMethod,
					SelectedManga);
			}).Wait();
			_mangas.Remove(SelectedManga);
		}

		private void HandleUndoMethod(Manga manga) {
			Task.Run(async () => { await new Database().InsertManga(manga); });
			_mangas.Add(manga);
		}

		private void RefreshManga() {
			throw new NotImplementedException();
		}

		private void ViewManga() {
			throw new NotImplementedException();
		}

		private List<SiteModel> FillSites() {
			PluginHost.Instance.Initialize();
			return new List<SiteModel>(PluginHost.Instance.Sites.Select(s=> new SiteModel(s)));
		}
	}
}