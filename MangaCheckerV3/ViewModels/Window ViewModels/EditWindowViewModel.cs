using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
using MangaCheckerV3.Common;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Window_ViewModels {
    [ImplementPropertyChanged]
    public class EditWindowViewModel {
        private readonly ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();

        public EditWindowViewModel(Manga manga) {
            Manga = manga;
            DeleteGenreCommand = new ActionCommand(DeleteGenre);
            AddGenreCommand = new ActionCommand(AddGenre);
            SaveMangaCommand = new ActionCommand(SaveManga);
            GenresAdded = new ReadOnlyObservableCollection<Genre>(_genres);
            SiteSelected = Manga.Site;
            SelectedGenre = Genres[0];
            manga.Genres.ForEach(_genres.Add);
        }

        public Manga Manga { get; set; }
        public List<Genre> Genres => Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        public ReadOnlyObservableCollection<Genre> GenresAdded { get; }

        public List<string> Sites => ProviderService.Providers.Select(p => p.DbName).ToList();
        public string SiteSelected { get; set; }

        public Genre SelectedGenre { get; set; }

        public ICommand AddGenreCommand { get; }

        public ICommand DeleteGenreCommand { get; }

        public ICommand SaveMangaCommand { get; }

        private void AddGenre() {
            if (Manga.Genres.Contains(SelectedGenre)) return;
            Manga.Genres.Add(SelectedGenre);
            _genres.Add(SelectedGenre);
        }

        private void DeleteGenre(object genre) {
            var enumVal = (Genre) Enum.Parse(typeof(Genre), genre.ToString());
            if (!Manga.Genres.Contains(enumVal)) return;
            Manga.Genres.Remove(enumVal);
            _genres.Remove(enumVal);
        }

        private void SaveManga() {
            try {
                LiteDB.Update(Manga);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}