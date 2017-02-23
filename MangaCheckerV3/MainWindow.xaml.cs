using System.ComponentModel;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private IPluginHost _pluginHost;
        public MainWindow(IProviderService providerService, ILinkParser linkParser, IWindowFactory windowFactory,
            IViewModelFactory viewModelFactory, ILiteDb liteDb, Logger logger, IPluginHost pluginHost) {
            InitializeComponent();
            _pluginHost = pluginHost;
            DataContext = new MainWindowViewModel(providerService, linkParser, windowFactory , viewModelFactory, liteDb, logger);
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var ctx = (MainWindowViewModel) DataContext;
            ctx.Dispose();
            _pluginHost.Dispose();
        }
    }
}