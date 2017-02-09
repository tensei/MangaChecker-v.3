using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MangaChecker.Database;
using MangaChecker.Utilities;
using MangaCheckerV3.Common;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private async void AppStartup(object sender, StartupEventArgs args) {
            //if (!Debugger.IsAttached)
            //	ExceptionHandler.AddGlobalHandlers();
            //ThemeHelper.ChangeAccentColorTo("red", "red", false);
            PluginHost.Instance.Initialize();
            var p = new ProviderService();
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db"))) {
                LiteDb.CheckDbVersion(p.Providers);
            }
            else {
                LiteDb.CreateDatabase(p.Providers);
            }

            var mainWindow = new MainWindow {
                DataContext = new MainWindowViewModel(p)
            };
            ThemeViewModel.Instance.SetupTheme();
            await Task.Delay(400);
            mainWindow.ShowDialog();
        }
    }
}