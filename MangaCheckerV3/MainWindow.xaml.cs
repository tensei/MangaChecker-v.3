using System.ComponentModel;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.ViewModels;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow(IProviderService providerService, IViewModelFactory viewModelFactory, ILiteDb liteDb, Logger logger) {
            InitializeComponent();
            DataContext = new MainWindowViewModel(providerService, viewModelFactory, liteDb, logger);
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var ctx = (MainWindowViewModel) DataContext;
            ctx.Dispose();
        }
    }
}