using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
using MangaCheckerV3.Common;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class NewMangaViewModel {
        private readonly ObservableCollection<Manga> _newManga = new ObservableCollection<Manga>();

        public NewMangaViewModel() {
            ViewCommand = new ActionCommand(v => View((Manga) v));
            RemoveCommand = new ActionCommand(r => Remove((Manga) r));
            RemoveAllCommand = new ActionCommand(RemoveAll);
            RefreshListCommand = new ActionCommand(GetNewMangas);
            NewManga = new ReadOnlyObservableCollection<Manga>(_newManga);
            LiteDb.MangaEvent += DatabaseOnMangaEvent;
            GetNewMangas();
        }

        public ReadOnlyObservableCollection<Manga> NewManga { get; }

        public ICommand ViewCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand RemoveAllCommand { get; }

        public string LastRefresh { get; set; }

        private void View(Manga manga) {
            Utilities.OpenViewer(manga);
        }

        private void Remove(Manga manga) {
            LiteDb.DeleteNewManga(manga);
            _newManga.Remove(manga);
        }

        private void RemoveAll() {
            if (_newManga.Count == 0) return;
            var nm = _newManga.ToList();
            foreach (var manga in nm) LiteDb.DeleteNewManga(manga);
            _newManga.Clear();
        }

        private void GetNewMangas() {
            if (_newManga.Count > 0) _newManga.Clear();
            var nm = LiteDb.GetAllNewMangas();
            nm?.ToList().ForEach(_newManga.Add);
            LastRefresh = DateTime.Now.ToLongTimeString();
        }

        private void DatabaseOnMangaEvent(object sender, MangaEnum mangaEnum) {
            if (mangaEnum != MangaEnum.Update) return;
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
            LiteDb.InsertNewManga(nm);
            GetNewMangas();
        }
    }
}