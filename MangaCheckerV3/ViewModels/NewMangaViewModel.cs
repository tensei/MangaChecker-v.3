using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enum;
using MangaChecker.Data.Interface;
using MangaChecker.Data.Model;
using MangaChecker.Database;
using MangaCheckerV3.Common;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class NewMangaViewModel {
        private readonly ObservableCollection<Manga> _newManga = new ObservableCollection<Manga>();
        private readonly Utilities _utilities;
        private readonly ILiteDb _liteDb;
        public NewMangaViewModel(Utilities utilities, ILiteDb liteDb) {
            _utilities = utilities;
            _liteDb = liteDb;
            ViewCommand = new ActionCommand(v => View((Manga) v));
            RemoveCommand = new ActionCommand(r => Remove((Manga) r));
            RemoveAllCommand = new ActionCommand(RemoveAll);
            RefreshListCommand = new ActionCommand(GetNewMangas);
            NewManga = new ReadOnlyObservableCollection<Manga>(_newManga);
            _liteDb.MangaEvent += DatabaseOnMangaEvent;
            GetNewMangas();
        }

        public ReadOnlyObservableCollection<Manga> NewManga { get; }

        public ICommand ViewCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand RemoveAllCommand { get; }

        public string LastRefresh { get; set; }

        private void View(Manga manga) {
            _utilities.OpenViewer(manga);
        }

        private void Remove(Manga manga) {
            _liteDb.DeleteNewManga(manga);
            _newManga.Remove(manga);
        }

        private void RemoveAll() {
            if (_newManga.Count == 0) {
                return;
            }
            var nm = _newManga.ToList();
            foreach (var manga in nm) {
                _liteDb.DeleteNewManga(manga);
            }
            _newManga.Clear();
        }

        private void GetNewMangas() {
            if (_newManga.Count > 0) {
                _newManga.Clear();
            }
            var nm = _liteDb.GetAllNewMangas();
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
            _liteDb.InsertNewManga(nm);
            GetNewMangas();
        }
    }
}