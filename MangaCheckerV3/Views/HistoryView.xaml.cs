using System.Windows.Controls;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Views {
    /// <summary>
    ///     Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : UserControl {
        public HistoryView() {
            InitializeComponent();
            DataContext = new HistoryViewModel();
        }
    }
}