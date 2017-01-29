using System.Windows.Input;
using MangaCheckerV3.Common;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class MainWindowViewModel {
        private readonly ProviderService _providerService;
        public static MainWindowViewModel Instance { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel() {
            Instance = this;
            SnackbarQueue = new SnackbarMessageQueue();
            SnackbarQueue.Enqueue("Starting...", true);

            StartStopCommand = new ActionCommand(StartStop);
            RefreshCommand = new ActionCommand(() => _providerService.Timer = 5);

            _providerService = new ProviderService();
            _providerService.Run();
            //var xml = new Client().GetRssFeedAsync("https://yomanga.co/reader/feeds/rss").ConfigureAwait(false);
            //      new Client().GetRssFeedAsync("https://gameofscanlation.moe/projects/trinity-wonder/index.rss").ConfigureAwait(false);
            //      new Client().GetRssFeedAsync("http://www.webtoons.com/en/fantasy/tower-of-god/rss?title_no=95").ConfigureAwait(false);
        }

        public SnackbarMessageQueue SnackbarQueue { get; }

        public int DrawerIndex { get; set; }
        public bool MenuToggleButton { get; set; } = true;

        public ICommand StartStopCommand { get; }
        public ICommand RefreshCommand { get; }
        public PackIconKind PausePlayButtonIcon { get; set; } = PackIconKind.Pause;

        public int Timer => _providerService.Timer;

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