using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;

namespace MangaChecker.ViewModels.ViewModels {
    public class NewMangaViewModel : INotifyPropertyChanged {
        private readonly IDbContext _dbContext;
        private readonly ObservableCollection<Manga> _newManga = new ObservableCollection<Manga>();
        private readonly IWindowFactory _windowFactory;

        public NewMangaViewModel(IWindowFactory windowFactory, IDbContext dbContext) {
            _windowFactory = windowFactory;
            _dbContext = dbContext;
            ViewCommand = new ActionCommand(v => View((Manga) v));
            RemoveCommand = new ActionCommand(r => Remove((Manga) r));
            RemoveAllCommand = new ActionCommand(RemoveAll);
            RefreshListCommand = new ActionCommand(GetNewMangas);
            NewManga = new ReadOnlyObservableCollection<Manga>(_newManga);
            _dbContext.MangaEvent += DatabaseOnMangaEvent;
            GetNewMangas();
        }

        public ReadOnlyObservableCollection<Manga> NewManga { get; }

        public ICommand ViewCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand RemoveAllCommand { get; }

        public string LastRefresh { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void View(IManga manga) {
            _windowFactory.CreateViewerWindow(manga);
        }

        private void Remove(Manga manga) {
            _dbContext.DeleteNewManga(manga);
            _newManga.Remove(manga);
        }

        private void RemoveAll() {
            if (_newManga.Count == 0) {
                return;
            }
            var nm = _newManga.ToList();
            foreach (var manga in nm) {
                _dbContext.DeleteNewManga(manga);
            }
            _newManga.Clear();
        }

        private void GetNewMangas() {
            if (_newManga.Count > 0) {
                _newManga.Clear();
            }
            var nm = _dbContext.GetAllNewMangas();
            nm?.ToList().ForEach(_newManga.Add);
            LastRefresh = DateTime.Now.ToLongTimeString();
        }

        private void DatabaseOnMangaEvent(object sender, MangaEnum mangaEnum) {
            if (mangaEnum != MangaEnum.Update) {
                return;
            }
            var m = (Manga) sender;
            var nm = new Manga {
                Name = m.Name,
                Chapter = m.Chapter,
                Added = DateTime.Now,
                Updated = m.Updated,
                Link = m.Link,
                Rss = m.Rss,
                Site = m.Site,
                SpecialLink = m.SpecialLink,
                Genres = m.Genres,
                New = m.New,
                OtherChapters = m.OtherChapters,
                Newest = m.Newest
            };
            _newManga.Add((Manga) sender);
            _dbContext.InsertNewManga(nm);
            GetNewMangas();
        }
    }
}