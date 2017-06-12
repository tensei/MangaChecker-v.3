using System.ComponentModel;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly IPluginHost _pluginHost;

        public MainWindow(IProviderService providerService, ILinkParser linkParser, IWindowFactory windowFactory,
            IViewModelFactory viewModelFactory, IDbContext dbContext, Logger logger, IPluginHost pluginHost) {
            InitializeComponent();
            _pluginHost = pluginHost;
            DataContext = new MainWindowViewModel(providerService, linkParser, windowFactory, viewModelFactory, dbContext,
                logger);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var ctx = (MainWindowViewModel) DataContext;
            ctx.Dispose();
            _pluginHost.Dispose();
        }
    }
}