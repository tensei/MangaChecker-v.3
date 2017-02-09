using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enum;
using MangaChecker.Data.Interface;
using MangaChecker.Data.Model;
using MangaChecker.Database;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Adding_ViewModels {
    [ImplementPropertyChanged]
    public class AdvancedViewModel {
        private readonly ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();

        private readonly IProviderService ProviderService;

        public AdvancedViewModel(IProviderService providerService) {
            ProviderService = providerService;
            Manga = new Manga();
            DeleteGenreCommand = new ActionCommand(DeleteGenre);
            AddGenreCommand = new ActionCommand(AddGenre);
            AddMangaCommand = new ActionCommand(AddManga);
            GenresAdded = new ReadOnlyObservableCollection<Genre>(_genres);
            SiteSelected = Sites?[0];
            SelectedGenre = Genres[0];
        }

        public Manga Manga { get; set; }
        public List<Genre> Genres => Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        public ReadOnlyObservableCollection<Genre> GenresAdded { get; }

        public List<string> Sites => ProviderService.Providers.Select(p => p.DbName).ToList();
        public string SiteSelected { get; set; }

        public Genre SelectedGenre { get; set; }

        public ICommand AddGenreCommand { get; }

        public ICommand DeleteGenreCommand { get; }

        public ICommand AddMangaCommand { get; }

        private void AddGenre() {
            if (Manga.Genres.Contains(SelectedGenre)) {
                return;
            }
            Manga.Genres.Add(SelectedGenre);
            _genres.Add(SelectedGenre);
        }

        private void DeleteGenre(object genre) {
            var enumVal = (Genre) Enum.Parse(typeof(Genre), genre.ToString());
            if (!Manga.Genres.Contains(enumVal)) {
                return;
            }
            Manga.Genres.Remove(enumVal);
            _genres.Remove(enumVal);
        }

        private void AddManga() {
            try {
                Manga.Site = SiteSelected;
                LiteDb.InsertManga(Manga);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}