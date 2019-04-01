using System.ComponentModel;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;

namespace MangaCheckerV3
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow(IProviderService providerService, ILinkParser linkParser, IWindowFactory windowFactory,
            IViewModelFactory viewModelFactory, IDbContext dbContext, Logger logger)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(providerService, linkParser, windowFactory, viewModelFactory,
                dbContext,
                logger);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var ctx = (MainWindowViewModel) DataContext;
            ctx.Dispose();
        }
    }
}