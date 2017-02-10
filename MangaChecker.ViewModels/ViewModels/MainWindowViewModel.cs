using System;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Utilities;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels {
    [ImplementPropertyChanged]
    public class MainWindowViewModel : IDisposable{
        public static MainWindowViewModel Instance;

        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IProviderService providerService, IViewModelFactory viewModelFactory, ILiteDb liteDb, Logger logger) {
            _liteDb = liteDb;
            _logger = logger;
            Instance = this;
            ProviderService = providerService;
            _viewModelFactory = viewModelFactory;
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
            _logger.Log.Info("starting");
            //SnackbarQueue.Enqueue("Starting...", true);
        }

        private readonly ILiteDb _liteDb;
        private readonly Logger _logger;
        public IProviderService ProviderService { get; }
        private readonly IViewModelFactory _viewModelFactory;

        public ISnackbarMessageQueue SnackbarQueue { get; }

        public int DrawerIndex { get; set; }
        public bool MenuToggleButton { get; set; } = true;

        public ICommand StartStopCommand { get; }
        public ICommand RefreshCommand { get; }
        public string PausePlayButtonIcon { get; set; } = "Pause";

        public MangaListViewModel MangaListContext => _viewModelFactory.CreateMangaListViewModel;
        public AddMangaViewModel AddContext => _viewModelFactory.CreateAddMangaViewModel;
        public SettingsViewModel SettingsContext => _viewModelFactory.CreateSettingsViewModel;
        public PluginsViewModel PluginsContext => _viewModelFactory.CreatePluginsViewModel;
        public ThemeViewModel ThemeContext => _viewModelFactory.CreateThemeViewModel;
        public NewMangaViewModel NewContext => _viewModelFactory.CreateNewMangaViewModel;
        public HistoryViewModel HistoryContext => _viewModelFactory.CreateHistoryViewModel;

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