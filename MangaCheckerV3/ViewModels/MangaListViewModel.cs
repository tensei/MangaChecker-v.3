﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using MangaCheckerV3.ViewModels.Window_ViewModels;
using MangaCheckerV3.Views.Windows;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class MangaListViewModel {
        /// <summary>
        ///     Initializes a new instance of the MangaListViewModel class.
        /// </summary>
        private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

        private SiteListItem _selectedSite;
        private string _sortMode = "Updated";

        public MangaListViewModel() {
            Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
            IncreaseCommand = new ActionCommand(IncreaseChapter);
            DecreaseCommand = new ActionCommand(DecreaseChapter);
            DeleteCommand = new ActionCommand(DeleteManga);
            OpenMangaCommand = new ActionCommand(OpenMangaSite);
            RefreshCommand = new ActionCommand(async () => await RefreshManga());
            ViewerCommand = new ActionCommand(async () => await ViewManga());
            DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
            RefreshListCommand = new ActionCommand(() => FillMangaList(SelectedSite.Name));
            EditCommand = new ActionCommand(EditManga);
            LiteDB.SettingEvent += DatabaseOnSettingEvent;
        }

        public List<SiteListItem> Sites { get; set; } = new List<SiteListItem> {
            new SiteListItem {Name = "All", Overrideable = false, IsEnabled = 1},
            new SiteListItem {Name = "Mangareader"},
            new SiteListItem {Name = "Mangafox"},
            new SiteListItem {Name = "Mangahere"},
            new SiteListItem {Name = "Mangastream"},
            new SiteListItem {Name = "Batoto"},
            new SiteListItem {Name = "Kissmanga"},
            new SiteListItem {Name = "Webtoons"},
            new SiteListItem {Name = "YoManga"},
            new SiteListItem {Name = "GameOfScanlation"},
            new SiteListItem {Name = "KireiCake"},
            new SiteListItem {Name = "Jaiminisbox"},
            new SiteListItem {Name = "HeyManga"},
            new SiteListItem {Name = "Tomochan"},
            new SiteListItem {Name = "Crunchyroll"},
            new SiteListItem {Name = "Backlog", Overrideable = false, IsEnabled = 1}
        }.OrderBy(s => s.Name).ToList();

        public ReadOnlyObservableCollection<Manga> Mangas { get; }


        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenMangaCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ViewerCommand { get; }
        public ICommand DeselectCommand { get; }
        public ICommand RefreshListCommand { get; }
        public ICommand EditCommand { get; }


        public Manga SelectedManga { get; set; }

        public SiteListItem SelectedSite {
            get { return _selectedSite; }
            set {
                _selectedSite = value;
                FillMangaList(value.Name);
            }
        }

        public int SelectedSiteIndex { get; set; }

        public string SortMode {
            get { return _sortMode; }
            set {
                if (_sortMode == value) return;
                _sortMode = value;
                Sortby(value, _mangas.ToList()).ForEach(_mangas.Add);
            }
        }

        public int AmountItem { get; set; } = 1;

        private void DatabaseOnSettingEvent(object sender, SettingEnum settingEnum) {
            if (settingEnum != SettingEnum.Get && settingEnum != SettingEnum.Update) return;
            var settings = sender as List<Settings>;
            if (settings == null) return;
            foreach (var setting in settings) {
                var v = Sites.FirstOrDefault(s => s.Name == setting.Setting);
                if (v != null) v.IsEnabled = setting.Active;
            }
        }


        private List<Manga> Sortby(string sort, List<Manga> mangas) {
            var m = mangas;
            if (_mangas.Count > 0) _mangas.Clear();
            switch (sort.ToLower()) {
                case "updated":
                    return m?.OrderByDescending(x => x.Updated).ToList();
                case "name":
                    return m?.OrderBy(x => x.Name).ToList();
                case "added":
                    return m?.OrderBy(x => x.Added).ToList();
                case "site":
                    return m?.OrderBy(x => x.Site).ToList();
                case "chapter":
                    return m?.OrderBy(x => x.Chapter).ToList();
                default:
                    return m;
            }
        }
        
        private void Fill() {
            var x = Sortby(_sortMode, LiteDB.GetAllMangas().ToList());
            x.ForEach(_mangas.Add);
        }

        private void IncreaseChapter() {
            SelectedManga.Chapter += AmountItem;
            SelectedManga.Newest = SelectedManga.Chapter.ToString(CultureInfo.InvariantCulture);
            SelectedManga.Updated = DateTime.Now;
            LiteDB.Update(SelectedManga, true);
        }

        private void DecreaseChapter() {
            SelectedManga.Chapter -= AmountItem;
            SelectedManga.Newest = SelectedManga.Chapter.ToString(CultureInfo.InvariantCulture);
            SelectedManga.Updated -= TimeSpan.FromDays(1);
            LiteDB.Update(SelectedManga, true);
        }

        private void OpenMangaSite() {
            if (!SelectedManga.Link.Contains("/")) return;
            Process.Start(SelectedManga.Link);
        }

        private void DeleteManga() {
            LiteDB.Delete(SelectedManga);
            GlobalVariables.SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO", HandleUndoMethod,
                SelectedManga);
            _mangas.Remove(SelectedManga);
        }

        private void HandleUndoMethod(Manga manga) {
            LiteDB.InsertManga(manga);
            _mangas.Add(manga);
        }

        private async Task RefreshManga() {
            var provider = ProviderService.Providers.Find(p => p.DbName == SelectedManga.Site);
            await provider.CheckOne(SelectedManga);
        }

        private async Task ViewManga() {
//#if DEBUG
//            var imgs = new List<object>(Directory.GetFiles(@"D:\Pictures", "*.png"));
//#else
            var provider = ProviderService.Providers.Find(p => p.DbName == SelectedManga.Site);
            if (!provider.ViewEnabled) {
                return;
            }
            var imgs = await provider.GetImagesTaskAsync(SelectedManga.Link);
//#endif
            var viewer = new ViewerWindow {
                DataContext = new ViewerWindowViewModel(imgs.Item1, SelectedManga, imgs.Item2),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            viewer.Show();
        }

        private void EditManga() {
            var e = new EditWindow {
                DataContext = new EditWindowViewModel(SelectedManga),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            e.Show();
        }

        private void FillMangaList(string site) {
            switch (site) {
                case "All":
                    Fill();
                    break;
                default:
                    Sortby(_sortMode, LiteDB.GetMangasFrom(site).ToList()).ForEach(_mangas.Add);
                    break;
            }
        }
    }
}