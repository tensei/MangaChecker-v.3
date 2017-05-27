using System;
using System.ComponentModel;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.ViewModels.ViewModels.Window_ViewModels;
using MangaCheckerV3.Properties;

namespace MangaCheckerV3.Views.Windows {
    /// <summary>
    ///     Interaction logic for ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow {
        public ViewerWindow(IManga manga, IProvider provider, bool saveEnabled = true) {
            InitializeComponent();
            DataContext = new ViewerWindowViewModel(manga, provider, Close, saveEnabled);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                DragMove();
            }
        }

        private void ViewerWindow_OnClosing(object sender, CancelEventArgs e) {
            var data = (ViewerWindowViewModel) DataContext;
            data?.Dispose();
            GC.Collect();
            DataContext = null;
            Settings.Default.ViewerHeight = Height;
            Settings.Default.ViewerWidth = Width;
            Settings.Default.Save();
        }
    }
}