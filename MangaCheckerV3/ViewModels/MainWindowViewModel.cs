using System;
using System.Windows.Input;
using MangaChecker.Data.Interface;
using MangaChecker.Utilities;
using MangaCheckerV3.Common;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class MainWindowViewModel : IDisposable{
        public static MainWindowViewModel Instance;

        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IProviderService providerService,
            AddMangaViewModel addMangaViewModel,
            MangaListViewModel mangaListViewModel,
            SettingsViewModel settingsViewModel,
            NewMangaViewModel newMangaViewModel,
            HistoryViewModel historyViewModel, 
            ThemeViewModel themeViewModel, 
            PluginsViewModel pluginsViewModel, ILiteDb liteDb) {
            _liteDb = liteDb;
            Instance = this;
            ProviderService = providerService;
            GlobalVariables.ProviderService = providerService;
            MangaListContext = mangaListViewModel;
            AddContext = addMangaViewModel;
            SettingsContext = settingsViewModel;
            NewContext = newMangaViewModel;
            HistoryContext = historyViewModel;
            ThemeContext = themeViewModel;
            PluginsContext = pluginsViewModel;
            SnackbarQueue = new SnackbarMessageQueue();
            StartStopCommand = new ActionCommand(StartStop);
            RefreshCommand = new ActionCommand(() => ProviderService.Timer = 5);

            ProviderService.Run().ConfigureAwait(false);

            //test PEPE
            //WebParser.GetRssFeedAsync("https://yomanga.co/reader/feeds/rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("https://gameofscanlation.moe/projects/trinity-wonder/index.rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://bato.to/myfollows_rss?secret=dd5831f7430c7ed7ea7055db4fe7b7ad&l=English").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://read.tomochan.today/rss").ConfigureAwait(false);
            //WebParser.GetRssFeedAsync("http://www.webtoons.com/en/fantasy/tower-of-god/rss?title_no=95").ConfigureAwait(false);
            Logger.Log.Info("starting");
            SnackbarQueue.Enqueue("Starting...", true);
        }

        private ILiteDb _liteDb;
        public IProviderService ProviderService { get; }

        public ISnackbarMessageQueue SnackbarQueue { get; }

        public int DrawerIndex { get; set; }
        public bool MenuToggleButton { get; set; } = true;

        public ICommand StartStopCommand { get; }
        public ICommand RefreshCommand { get; }
        public string PausePlayButtonIcon { get; set; } = "Pause";

        public MangaListViewModel MangaListContext { get; }

        public AddMangaViewModel AddContext { get; }

        public SettingsViewModel SettingsContext { get; }

        public PluginsViewModel PluginsContext { get; }

        public ThemeViewModel ThemeContext { get; }

        public NewMangaViewModel NewContext { get; }

        public HistoryViewModel HistoryContext { get; }

        private void StartStop() {
            if (!ProviderService.Pause) {
                PausePlayButtonIcon = "Play";
                ProviderService.Pause = true;
                return;
            }
            ProviderService.Pause = false;
            PausePlayButtonIcon = "Pause";
        }

        public void Dispose() {
            _liteDb.Dispose();
        }
    }
}