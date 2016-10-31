using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.ViewModels;
using MangaCheckerV3.ViewModels.Settings_ViewModels;

namespace MangaCheckerV3 {
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private async void AppStartup(object sender, StartupEventArgs args) {
			//if (!Debugger.IsAttached)
			//	ExceptionHandler.AddGlobalHandlers();
			//ThemeHelper.ChangeAccentColorTo("red", "red", false);
			var mainWindow = new MainWindow {
				DataContext = new MainWindowViewModel()
			};
			await ThemeViewModel.Instance.SetupTheme();
			await Task.Delay(400);
			mainWindow.Show();
		}
	}
}