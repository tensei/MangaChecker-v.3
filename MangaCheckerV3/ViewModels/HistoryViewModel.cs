using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
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
            Database.MangaEvent += DatabaseOnMangaEvent;
            Refresh();
        }

        private void DatabaseOnMangaEvent(object sender, MangaEnum mangaEnum) {
            if (mangaEnum != MangaEnum.Update) return;
            _history.Add((Manga) sender);
            Database.InsertHistory((Manga)sender);
        }

        public ReadOnlyObservableCollection<Manga> History { get; }

        public ICommand RefreshListCommand { get; }
        public ICommand RemoveCommand { get; }

        public ICommand ViewCommand { get; }

        public string LastRefresh { get; set; }

        private void Refresh() {
            if(_history.Count > 0) _history.Clear();
            Database.GetHistory()?.ToList().ForEach(_history.Add);
            LastRefresh = DateTime.Now.ToLongTimeString();
        }

        private void Remove(Manga manga) {
            Database.DeleteHistory(manga);
            _history.Remove(manga);
        }

        private void View(Manga manga) {
            throw new NotImplementedException(manga.Name);
        }
    }
}