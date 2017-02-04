using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MangaCheckerV3.ViewModels.Window_ViewModels;

namespace MangaCheckerV3.Views.Windows {
    /// <summary>
    /// Interaction logic for ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow {
        public ViewerWindow() {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if(Trans.SelectedIndex != 0) return;
            var data = (ViewerWindowViewModel) DataContext;
            switch (e.Key) {
                case Key.D:
                case Key.Right:
                case Key.Space:
                    data.NextPageCommand.Execute(null);
                    break;
                case Key.A:
                case Key.Left:
                case Key.Back:
                    data.PrevPageCommand.Execute(null);
                    break;
            }
        }

        private void ViewerWindow_OnClosing(object sender, CancelEventArgs e) {

            var data = (ViewerWindowViewModel)DataContext;
            data.Dispose();
            GC.Collect();
            DataContext = null;
        }
    }
}
