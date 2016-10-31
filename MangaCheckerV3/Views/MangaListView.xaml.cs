using System.Windows.Controls;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Views {
	/// <summary>
	///     Interaction logic for MangaListView.xaml
	/// </summary>
	public partial class MangaListView : UserControl {
		public MangaListView() {
			InitializeComponent();
			DataContext = new MangaListViewModel();
		}
	}
}