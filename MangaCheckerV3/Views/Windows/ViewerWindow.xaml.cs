using System;
using System.ComponentModel;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MangaCheckerV3.Properties;
using MangaCheckerV3.ViewModels.Window_ViewModels;

namespace MangaCheckerV3.Views.Windows {
    /// <summary>
    ///     Interaction logic for ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow {
        public ViewerWindow() {
            InitializeComponent();
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