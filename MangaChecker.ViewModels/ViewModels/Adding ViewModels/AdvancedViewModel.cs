using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.ViewModels.ViewModels.Adding_ViewModels
{
    public class AdvancedViewModel : INotifyPropertyChanged
    {
        private readonly IDbContext _dbContext;
        private readonly ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();

        public AdvancedViewModel(IProviderSet providerService, IDbContext dbContext)
        {
            _dbContext = dbContext;
            Manga = new Manga();
            DeleteGenreCommand = new ActionCommand(DeleteGenre);
            AddGenreCommand = new ActionCommand(AddGenre);
            AddMangaCommand = new ActionCommand(AddManga);
            GenresAdded = new ReadOnlyObservableCollection<Genre>(_genres);
            SelectedGenre = Genres[0];

            Sites = providerService.GetAll.Select(p => p.DbName).ToList();
            Sites?.Add("Backlog");

            SiteSelected = Sites?[0];
        }

        public Manga Manga { get; set; }
        public List<Genre> Genres => Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        public ReadOnlyObservableCollection<Genre> GenresAdded { get; }

        public List<string> Sites { get; }
        public string SiteSelected { get; set; }

        public Genre SelectedGenre { get; set; }

        public ICommand AddGenreCommand { get; }

        public ICommand DeleteGenreCommand { get; }

        public ICommand AddMangaCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void AddGenre()
        {
            if (Manga.Genres.Contains(SelectedGenre))
            {
                return;
            }
            Manga.Genres.Add(SelectedGenre);
            _genres.Add(SelectedGenre);
        }

        private void DeleteGenre(object genre)
        {
            var enumVal = (Genre) Enum.Parse(typeof(Genre), genre.ToString());
            if (!Manga.Genres.Contains(enumVal))
            {
                return;
            }
            Manga.Genres.Remove(enumVal);
            _genres.Remove(enumVal);
        }

        private void AddManga()
        {
            try
            {
                Manga.Site = SiteSelected;
                _dbContext.InsertManga(Manga);
                Manga = new Manga();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}