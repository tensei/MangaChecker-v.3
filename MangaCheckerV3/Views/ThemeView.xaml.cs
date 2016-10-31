using System.Windows.Controls;
using MangaCheckerV3.ViewModels.Settings_ViewModels;

namespace MangaCheckerV3.Views {
	/// <summary>
	/// Interaction logic for ThemeView.xaml
	/// </summary>
	public partial class ThemeView : UserControl {
		public ThemeView() {
			InitializeComponent();
			DataContext = new ThemeViewModel();
		}
	}
}
