using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}