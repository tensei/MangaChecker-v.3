using System.Windows.Controls;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Views {
    /// <summary>
    ///     Interaction logic for NewView.xaml
    /// </summary>
    public partial class NewView : UserControl {
        public NewView() {
            InitializeComponent();
            DataContext = new NewMangaViewModel();
        }
    }
}