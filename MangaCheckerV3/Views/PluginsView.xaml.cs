using System.Windows.Controls;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Views {
	/// <summary>
	///     Interaction logic for PluginsView.xaml
	/// </summary>
	public partial class PluginsView : UserControl {
		public PluginsView() {
			InitializeComponent();
			DataContext = new PluginsViewModel();
		}
	}
}