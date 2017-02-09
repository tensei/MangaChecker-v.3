using System.ComponentModel;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }
        private readonly ILiteDb _liteDb;
        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var ctx = (MainWindowViewModel) DataContext;
            ctx.Dispose();
        }
    }
}