﻿using System.Windows.Input;
using MangaChecker.Utilities;
using MangaCheckerV3.Common;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class MainWindowViewModel {
        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IProviderService providerService) {
            ProviderService = providerService;
            GlobalVariables.ProviderService = providerService;
            MangaListContext = new MangaListViewModel(providerService);
            AddContext = new AddMangaViewModel(providerService);
            SettingsContext = new SettingsViewModel();
            SnackbarQueue = GlobalVariables.SnackbarQueue;
            StartStopCommand = new ActionCommand(StartStop);
            RefreshCommand = new ActionCommand(() => ProviderService.Timer = 5);

            ProviderService.Run().ConfigureAwait(false);

            //test PEPE
            //WebParser.GetRssFeedAsync("https://yomanga.co/reader/feeds/rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("https://gameofscanlation.moe/projects/trinity-wonder/index.rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://bato.to/myfollows_rss?secret=dd5831f7430c7ed7ea7055db4fe7b7ad&l=English").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://read.tomochan.today/rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://www.webtoons.com/en/fantasy/tower-of-god/rss?title_no=95").ConfigureAwait(false);
            Log.Loggger.Info("starting");
            SnackbarQueue.Enqueue("Starting...", true);
        }

        public IProviderService ProviderService { get; }

        public SnackbarMessageQueue SnackbarQueue { get; }

        public int DrawerIndex { get; set; }
        public bool MenuToggleButton { get; set; } = true;

        public ICommand StartStopCommand { get; }
        public ICommand RefreshCommand { get; }
        public PackIconKind PausePlayButtonIcon { get; set; } = PackIconKind.Pause;

        public MangaListViewModel MangaListContext { get; }

        public AddMangaViewModel AddContext { get; }

        public SettingsViewModel SettingsContext { get; }

        private void StartStop() {
            if (!ProviderService.Pause) {
                PausePlayButtonIcon = PackIconKind.Play;
                ProviderService.Pause = true;
                return;
            }
            ProviderService.Pause = false;
            PausePlayButtonIcon = PackIconKind.Pause;
        }
    }
}