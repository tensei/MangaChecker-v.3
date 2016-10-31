using System.Windows.Controls;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Views {
	/// <summary>
	///     Interaction logic for SettingsView.xaml
	/// </summary>
	public partial class SettingsView : UserControl {
		public SettingsView() {
			InitializeComponent();
			DataContext = new SettingsViewModel();
		}
	}
}