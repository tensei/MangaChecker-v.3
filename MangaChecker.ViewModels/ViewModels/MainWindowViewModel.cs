using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;

namespace MangaChecker.ViewModels.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel Instance;

        private readonly IDbContext _dbContext;
        private readonly ILinkParser _linkParser;
        private readonly Logger _logger;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowFactory _windowFactory;

        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IProviderService providerService, ILinkParser linkParser,
            IWindowFactory windowFactory,
            IViewModelFactory viewModelFactory, IDbContext dbContext, Logger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _linkParser = linkParser;
            _windowFactory = windowFactory;
            Instance = this;
            ProviderService = providerService;
            _viewModelFactory = viewModelFactory;
            SnackbarQueue = new SnackbarMessageQueue();
            StartStopCommand = new ActionCommand(StartStop);
            RefreshCommand = new ActionCommand(() => ProviderService.Timer = 5);
            OpenViewerCommand = new ActionCommand(ParseLink);

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

        public IProviderService ProviderService { get; }

        public ISnackbarMessageQueue SnackbarQueue { get; }

        public int DrawerIndex { get; set; }
        public bool MenuToggleButton { get; set; } = true;

        public ICommand StartStopCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenViewerCommand { get; }
        public string PausePlayButtonIcon { get; set; } = "Pause";

        public MangaListViewModel MangaListContext => _viewModelFactory.CreateMangaListViewModel;
        public AddMangaViewModel AddContext => _viewModelFactory.CreateAddMangaViewModel;
        public SettingsViewModel SettingsContext => _viewModelFactory.CreateSettingsViewModel;
        public NewMangaViewModel NewContext => _viewModelFactory.CreateNewMangaViewModel;
        public HistoryViewModel HistoryContext => _viewModelFactory.CreateHistoryViewModel;

        public GalleryViewModel GalleryContext => _viewModelFactory.CreateGalleryViewModel;
        public event PropertyChangedEventHandler PropertyChanged;


        private void StartStop()
        {
            if (!ProviderService.Pause)
            {
                PausePlayButtonIcon = "Play";
                ProviderService.Pause = true;
                return;
            }
            ProviderService.Pause = false;
            PausePlayButtonIcon = "Pause";
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private void ParseLink()
        {
            var link = Clipboard.GetText().ToLower();
            if (string.IsNullOrWhiteSpace(link))
            {
                return;
            }
            var provider =
                _linkParser.GetProviderFirstOrDefault(p => p.LinkIsMatch(link) &&
                                                           link.ToLower().Contains(p.DbName.ToLower()) &&
                                                           p.ViewEnabled);
            if (provider != null)
            {
                _windowFactory.CreateViewerWindow(new Manga
                {
                    Name = link,
                    Link = link,
                    Site = provider.DbName
                }, provider);
            }
        }
    }
}