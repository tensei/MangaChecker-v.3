﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.ViewModels.ViewModels
{
    public class MangaListViewModel : INotifyPropertyChanged
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        ///     Initializes a new instance of the MangaListViewModel class.
        /// </summary>
        private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

        private readonly IProviderSet _providerService;

        private readonly ObservableCollection<SiteListItem> _sites = new ObservableCollection<SiteListItem>();
        private readonly IWindowFactory _windowFactory;

        private SiteListItem _selectedSite;
        private string _sortMode = "Updated";

        public MangaListViewModel(IProviderSet providerService, IWindowFactory windowFactory, IDbContext dbContext)
        {
            _providerService = providerService;
            _dbContext = dbContext;
            _windowFactory = windowFactory;
            SetupSites();
            //_dbContext.SettingEvent += DatabaseOnSettingEvent;
            Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
            Sites = new ReadOnlyObservableCollection<SiteListItem>(_sites);
            IncreaseCommand = new ActionCommand(IncreaseChapter);
            DecreaseCommand = new ActionCommand(DecreaseChapter);
            DeleteCommand = new ActionCommand(DeleteManga);
            OpenMangaCommand = new ActionCommand(OpenMangaSite);
            RefreshCommand = new ActionCommand(async () => await RefreshManga());
            ViewerCommand = new ActionCommand(ViewManga);
            DeselectCommand = new ActionCommand(() => { SelectedManga = null; });
            RefreshListCommand = new ActionCommand(() => FillMangaList(SelectedSite.Name));
            EditCommand = new ActionCommand(EditManga);
        }

        public ReadOnlyObservableCollection<Manga> Mangas { get; }
        public ReadOnlyObservableCollection<SiteListItem> Sites { get; }


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

        public SiteListItem SelectedSite
        {
            get => _selectedSite;
            set
            {
                _selectedSite = value;
                FillMangaList(value.Name);
            }
        }

        public int SelectedSiteIndex { get; set; }

        public string SortMode
        {
            get => _sortMode;
            set
            {
                if (_sortMode == value)
                {
                    return;
                }
                _sortMode = value;
                Sortby(value, _mangas.ToList()).ForEach(_mangas.Add);
            }
        }

        public int AmountItem { get; set; } = 1;
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetupSites()
        {
            var s = new List<SiteListItem>
            {
                new SiteListItem {Name = "Backlog", Overrideable = false, IsEnabled = 1},
                new SiteListItem {Name = "All", Overrideable = false, IsEnabled = 1}
            };
            _providerService.GetAll.ForEach(p => s.Add(new SiteListItem {Name = p.DbName, IsEnabled = 1}));
            s.OrderBy(x => x.Name).ToList().ForEach(_sites.Add);
        }

        //private void DatabaseOnSettingEvent(object sender, SettingEnum settingEnum) {
        //    if (settingEnum != SettingEnum.Get && settingEnum != SettingEnum.Update) {
        //        return;
        //    }
        //    var settings = sender as List<Settings>;
        //    if (settings == null) {
        //        return;
        //    }
        //    foreach (var setting in settings) {
        //        var v = _sites.FirstOrDefault(s => s.Name == setting.Setting);
        //        if (v != null) {
        //            v.IsEnabled = setting.Active;
        //        }
        //    }
        //}


        private List<Manga> Sortby(string sort, List<Manga> mangas)
        {
            var m = mangas;
            if (_mangas.Count > 0)
            {
                _mangas.Clear();
            }
            switch (sort.ToLower())
            {
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

        private void Fill()
        {
            var x = Sortby(_sortMode, _dbContext.GetAllMangas().ToList());
            x.ForEach(_mangas.Add);
        }

        private void IncreaseChapter()
        {
            SelectedManga.Chapter += AmountItem;
            SelectedManga.Newest = SelectedManga.Chapter.ToString(CultureInfo.InvariantCulture);
            SelectedManga.Updated = DateTime.Now;
            _dbContext.Update(SelectedManga, true);
        }

        private void DecreaseChapter()
        {
            SelectedManga.Chapter -= AmountItem;
            SelectedManga.Newest = SelectedManga.Chapter.ToString(CultureInfo.InvariantCulture);
            SelectedManga.Updated -= TimeSpan.FromDays(1);
            _dbContext.Update(SelectedManga, true);
        }

        private void OpenMangaSite()
        {
            if (!SelectedManga.Link.Contains("/"))
            {
                return;
            }
            Process.Start(SelectedManga.Link);
        }

        private void DeleteManga()
        {
            _dbContext.Delete(SelectedManga);
            MainWindowViewModel.Instance.SnackbarQueue.Enqueue($"Deleted {SelectedManga.Name}", "UNDO",
                HandleUndoMethod,
                SelectedManga);
            _mangas.Remove(SelectedManga);
        }

        private void HandleUndoMethod(Manga manga)
        {
            _dbContext.InsertManga(manga);
            _mangas.Add(manga);
        }

        private async Task RefreshManga()
        {
            var provider = _providerService.GetFirstOrDefault(p => p.DbName == SelectedManga.Site);
            await provider.CheckOne(SelectedManga);
        }

        private void ViewManga()
        {
            _windowFactory.CreateViewerWindow(SelectedManga);
        }

        private void EditManga()
        {
            _windowFactory.CreateEditWindow(SelectedManga);
        }

        private void FillMangaList(string site)
        {
            switch (site)
            {
                case "All":
                    Fill();
                    break;
                default:
                    Sortby(_sortMode, _dbContext.GetMangasFrom(site).ToList()).ForEach(_mangas.Add);
                    break;
            }
        }
    }
}