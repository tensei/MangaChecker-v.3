﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Database;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Window_ViewModels {
    [ImplementPropertyChanged]
    public class EditWindowViewModel {
        private readonly ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        private readonly ObservableCollection<string> _otherChapters = new ObservableCollection<string>();
        private readonly IProviderService _providerService;
        private readonly ILiteDb _liteDb;

        public EditWindowViewModel(Manga manga, IProviderService providerService, ILiteDb liteDb) {
            _providerService = providerService;
            _liteDb = liteDb;
            Manga = manga;
            DeleteGenreCommand = new ActionCommand(DeleteGenre);
            AddGenreCommand = new ActionCommand(AddGenre);
            AddOtherChapterCommand = new ActionCommand(AddOtherChapter);
            DeleteOtherChapterCommand = new ActionCommand(o => DeleteOtherChapter((string) o));
            SaveMangaCommand = new ActionCommand(SaveManga);
            GenresAdded = new ReadOnlyObservableCollection<Genre>(_genres);
            OtherChapters = new ReadOnlyObservableCollection<string>(_otherChapters);
            SiteSelected = Manga.Site;
            SelectedGenre = Genres[0];
            manga.Genres.ForEach(_genres.Add);
            manga.OtherChapters.ForEach(_otherChapters.Add);
        }

        public Manga Manga { get; set; }
        public List<Genre> Genres => Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        public ReadOnlyObservableCollection<Genre> GenresAdded { get; }

        public List<string> Sites => Enumerable.ToList<string>(_providerService.Providers.Select(p => p.DbName));
        public string SiteSelected { get; set; }
        public string OtherChapter { get; set; }

        public Genre SelectedGenre { get; set; }

        public ICommand AddGenreCommand { get; }

        public ICommand DeleteGenreCommand { get; }

        public ICommand SaveMangaCommand { get; }

        public ReadOnlyObservableCollection<string> OtherChapters { get; }

        public ICommand DeleteOtherChapterCommand { get; }

        public ICommand AddOtherChapterCommand { get; }

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

        private void AddOtherChapter() {
            if (Manga.OtherChapters.Contains(OtherChapter)) {
                return;
            }
            Manga.OtherChapters.Add(OtherChapter);
            _otherChapters.Add(OtherChapter);
        }

        private void DeleteOtherChapter(string otherChapter) {
            if (!Manga.OtherChapters.Contains(otherChapter)) {
                return;
            }
            Manga.OtherChapters.Remove(otherChapter);
            _otherChapters.Remove(otherChapter);
        }

        private void SaveManga() {
            try {
                _liteDb.Update(Manga);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}