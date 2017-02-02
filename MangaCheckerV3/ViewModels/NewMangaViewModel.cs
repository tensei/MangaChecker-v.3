using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class NewMangaViewModel {
        private readonly ObservableCollection<Manga> _newManga = new ObservableCollection<Manga>();

        public NewMangaViewModel() {
            ViewCommand = new ActionCommand(v => View((Manga) v));
            RemoveCommand = new ActionCommand(r => Remove((Manga) r));
            RemoveAllCommand = new ActionCommand(RemoveAll);
            RefreshListCommand = new ActionCommand(Refresh);
            NewManga = new ReadOnlyObservableCollection<Manga>(_newManga);
            LiteDB.MangaEvent += MangaEvent;
            Refresh();
        }

        public ReadOnlyObservableCollection<Manga> NewManga { get; }

        public ICommand ViewCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand RemoveAllCommand { get; }

        public string LastRefresh { get; set; }

        private void View(Manga manga) {
            throw new NotImplementedException();
        }

        private void Remove(Manga manga) {
            manga.New = false;
            _newManga.Remove(manga);
            LiteDB.Update(manga);
        }

        private void RemoveAll() {
            if (_newManga.Count == 0) return;
            var nm = _newManga.ToList();
            foreach (var manga in nm) {
                manga.New = false;
            }
            _newManga.Clear();
            LiteDB.UpdateTrans(nm, true);
        }

        private void Refresh() {
            if(_newManga.Count > 0)_newManga.Clear();
            var nm = LiteDB.GetAllNewMangas();
            nm?.ToList().ForEach(_newManga.Add);
            LastRefresh = DateTime.Now.ToLongTimeString();
        }

        private void MangaEvent(object manga, MangaEnum _event) {
            switch (_event) {
                case MangaEnum.Update:
                    Refresh();
                    break;
                case MangaEnum.Insert:
                    Refresh();
                    break;
            }
        }
    }
}