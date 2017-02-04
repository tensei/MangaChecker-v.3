using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MangaChecker.Database;
using MangaCheckerV3.Common;
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
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db"))) LiteDb.CheckDbVersion(ProviderService.Providers);
            else LiteDb.CreateDatabase(ProviderService.Providers);
            PluginHost.Instance.Initialize();
            var mainWindow = new MainWindow {
                DataContext = new MainWindowViewModel()
            };
            ThemeViewModel.Instance.SetupTheme();
            await Task.Delay(400);
            mainWindow.Show();
        }
    }
}