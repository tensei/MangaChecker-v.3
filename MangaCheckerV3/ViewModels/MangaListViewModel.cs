using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class MangaListViewModel {
		public static MangaListViewModel Instance;

		/// <summary>
		///     Initializes a new instance of the MangaListViewModel class.
		/// </summary>
		private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

		private readonly ObservableCollection<string> _sites = new ObservableCollection<string>();

		public readonly Dictionary<string, string> ListboxItemNames = new Dictionary<string, string> {
			{"All", null},
			{"Mangareader", null},
			{"Mangafox", null},
			{"Mangahere", null},
			{"Mangastream", null},
			{"Batoto", null},
			{"Kissmanga", null},
			{"Webtoons", null},
			{"YoManga", null},
			{"GoScanlation", "GameOfScanlation"},
			{"KireiCake", null},
			{"Jaiminisbox", null},
			{"HeyManga", null},
			{"Backlog", null}
			//{"DEBUG", null}
		};

		private string _selectedSite;

		public MangaListViewModel() {
			Instance = this;
			Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
			foreach (var key in ListboxItemNames.Keys) _sites.Add(key);
			Sites = new ReadOnlyObservableCollection<string>(_sites);
			Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
			IncreaseCommand = new ActionCommand(IncreaseChapter);
			DecreaseCommand = new ActionCommand(DecreaseChapter);
			DeleteCommand = new ActionCommand(DeleteManga);
			OpenMangaCommand = new ActionCommand(OpenMangaSite);
			RefreshCommand = new ActionCommand(RefreshManga);
			ViewerCommand = new ActionCommand(ViewManga);
			DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
			Fill();
		}

		public ReadOnlyObservableCollection<Manga> Mangas { get; }
		public ReadOnlyObservableCollection<string> Sites { get; }


		public ICommand IncreaseCommand { get; }
		public ICommand DecreaseCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand OpenMangaCommand { get; }
		public ICommand RefreshCommand { get; }
		public ICommand ViewerCommand { get; }
		public ICommand DeselectCommand { get; }


		public Manga SelectedManga { get; set; }

		public string SelectedSite {
			get { return _selectedSite; }
			set {
				_selectedSite = value;
				FillMangaList(value);
			}
		}

		public int SelectedSiteIndex { get; set; }


		private async void Fill() {
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

		private async void DeleteManga() {
			await new Database().Delete(SelectedManga);
			MainWindowViewModel.Instance.SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO", HandleUndoMethod,
				SelectedManga);
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

		private async void FillMangaList(string site) {
			if (_mangas.Count > 0) _mangas.Clear();

			switch (site) {
				case "All":
					foreach (var manga in await new Database().GetAllMangas()) _mangas.Add(manga);
					break;
				case "GoScanlation":
					foreach (var manga in await new Database().GetMangasFrom("GameOfScanlation")) _mangas.Add(manga);
					break;
				default:
					foreach (var manga in await new Database().GetMangasFrom(site)) _mangas.Add(manga);
					break;
			}
		}
	}
}