using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enum;
using MangaChecker.Data.Model;
using MangaChecker.Database;
using MangaCheckerV3.Common;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class HistoryViewModel {
        private readonly ObservableCollection<Manga> _history = new ObservableCollection<Manga>();

        public HistoryViewModel() {
            RefreshListCommand = new ActionCommand(Refresh);
            RemoveCommand = new ActionCommand(m => Remove((Manga) m));
            ViewCommand = new ActionCommand(m => View((Manga) m));
            History = new ReadOnlyObservableCollection<Manga>(_history);
            LiteDb.MangaEvent += DatabaseOnMangaEvent;
            Refresh();
        }

        public ReadOnlyObservableCollection<Manga> History { get; }

        public ICommand RefreshListCommand { get; }
        public ICommand RemoveCommand { get; }

        public ICommand ViewCommand { get; }

        public string LastRefresh { get; set; }

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
            _history.Insert(0, (Manga) sender);
            LiteDb.InsertHistory(nm);
        }

        private void Refresh() {
            if (_history.Count > 0) {
                _history?.Clear();
            }
            LiteDb.GetHistory()?.ToList().ForEach(_history.Add);
            LastRefresh = DateTime.Now.ToLongTimeString();
        }

        private void Remove(Manga manga) {
            LiteDb.DeleteHistory(manga);
            _history.Remove(manga);
        }

        private void View(Manga manga) {
            Utilities.OpenViewer(manga);
        }
    }
}