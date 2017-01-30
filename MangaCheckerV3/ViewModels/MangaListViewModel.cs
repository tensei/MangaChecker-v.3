using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class MangaListViewModel {
        public static MangaListViewModel Instance;

        public List<string> Sites => new List<string> {
            "All",
            "Mangareader",
            "Mangafox",
            "Mangahere",
            "Mangastream",
            "Batoto",
            "Kissmanga",
            "Webtoons",
            "YoManga",
            "GameOfScanlation",
            "KireiCake",
            "Jaiminisbox",
            "HeyManga",
            "Backlog"
        };

        /// <summary>
        ///     Initializes a new instance of the MangaListViewModel class.
        /// </summary>
        private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();
        
        private string _selectedSite;

        public MangaListViewModel() {
            Instance = this;
            Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
            Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
            IncreaseCommand = new ActionCommand(IncreaseChapter);
            DecreaseCommand = new ActionCommand(DecreaseChapter);
            DeleteCommand = new ActionCommand(DeleteManga);
            OpenMangaCommand = new ActionCommand(OpenMangaSite);
            RefreshCommand = new ActionCommand(RefreshManga);
            ViewerCommand = new ActionCommand(ViewManga);
            DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
            RefreshListCommand = new ActionCommand(() => FillMangaList(SelectedSite));
        }

        public ReadOnlyObservableCollection<Manga> Mangas { get; }


        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenMangaCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ViewerCommand { get; }
        public ICommand DeselectCommand { get; }
        public ICommand RefreshListCommand { get; }


        public Manga SelectedManga { get; set; }

        public string SelectedSite {
            get { return _selectedSite; }
            set {
                _selectedSite = value;
                FillMangaList(value);
            }
        }

        public int SelectedSiteIndex { get; set; }



        private void Fill() {
            var x = Database.GetAllMangas();
            foreach (var manga in x) _mangas.Add(manga);
        }

        private void IncreaseChapter() {
            SelectedManga.Chapter++;
            SelectedManga.Newest = SelectedManga.Chapter;
            SelectedManga.Updated = DateTime.Now;
            Database.Update(SelectedManga);
        }

        private void DecreaseChapter() {
            SelectedManga.Chapter--;
            SelectedManga.Newest = SelectedManga.Chapter;
            SelectedManga.Updated -= TimeSpan.FromDays(1);
            Database.Update(SelectedManga);
        }

        private void OpenMangaSite() {
            if (!SelectedManga.Link.Contains("/")) return;
            Process.Start(SelectedManga.Link);
        }

        private void DeleteManga() {
            Database.Delete(SelectedManga);
            MainWindowViewModel.Instance.SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO", HandleUndoMethod,
                SelectedManga);
            _mangas.Remove(SelectedManga);
        }

        private void HandleUndoMethod(Manga manga) {
            Database.InsertManga(manga);
            _mangas.Add(manga);
        }

        private void RefreshManga() {
            throw new NotImplementedException();
        }

        private void ViewManga() {
            throw new NotImplementedException();
        }

        private void FillMangaList(string site) {
            if (_mangas.Count > 0) _mangas.Clear();
            switch (site) {
                case "All":
                    Fill();
                    break;
                default:
                    Database.GetMangasFrom(site)?.ToList().ForEach(_mangas.Add);
                    break;
            }
        }
    }
}