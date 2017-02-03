using System.ComponentModel;
using MangaChecker.Database;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            LiteDB.Dispose();
        }
    }
}