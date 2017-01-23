
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;
using MangaCheckerV3.ViewModels;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Mangastream.ViewModel {
	[ImplementPropertyChanged]
	public class MainViewModel :ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

		private string _teststring;
		public ReadOnlyObservableCollection<Manga> Mangas { get; }

		public static MainViewModel Instance;

		public string Running { get; set; }

		public string Error { get; set; }

		public Manga SelectedManga { get; set; }
		public SnackbarMessageQueue SnackbarQueue { get; }


		public MainViewModel() {
			Instance = this;
			Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
			IncreaseCommand = new ActionCommand(IncreaseChapter);
			DecreaseCommand = new ActionCommand(DecreaseChapter);
			DeleteCommand = new ActionCommand(DeleteManga);
			OpenMangaCommand = new ActionCommand(OpenMangaSite);
			RefreshCommand = new ActionCommand(RefreshManga);
			ViewerCommand = new ActionCommand(ViewManga);
			DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
			SnackbarQueue = new SnackbarMessageQueue();
			Fill();
		}

		public ICommand IncreaseCommand { get; }
		public ICommand DecreaseCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand OpenMangaCommand { get; }
		public ICommand RefreshCommand { get; }
		public ICommand ViewerCommand { get; }
		public ICommand DeselectCommand { get; }


		private async void Fill() {
			var x = await new Database().GetMangasFrom("Mangastream");
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
			SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO", HandleUndoMethod, SelectedManga);
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
	}
}