using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaChecker.Providers;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaCheckerV3.Common;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Interfaces;
using MangaCheckerV3.ViewModels;
using MangaCheckerV3.Views.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;

namespace MangaCheckerV3 {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private async void AppStartup(object sender, StartupEventArgs args) {
            //if (!Debugger.IsAttached)
            //	ExceptionHandler.AddGlobalHandlers();
            //ThemeHelper.ChangeAccentColorTo("red", "red", false);

            var container = new UnityContainer();
            var db = new LiteDb();
            container.RegisterInstance<ILiteDb>(db);
            container.RegisterType<INewChapterHelper, NewChapterHelper>();
            container.RegisterType<IWebParser, WebParser>();
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<Logger>();
            var pluginhost = new PluginHost();
            container.RegisterInstance<IPluginHost>(pluginhost);
            //container.RegisterType<IWebParser, WebParser>();
            container.RegisterType<IProvider, Mangastream>("ms");
            container.RegisterType<IProvider, Batoto>("b");
            container.RegisterType<IProvider, Crunchyroll>("c");
            container.RegisterType<IProvider, GameOfScanlation>("g");
            container.RegisterType<IProvider, HeyManga>("h");
            container.RegisterType<IProvider, Jaiminisbox>("j");
            container.RegisterType<IProvider, KireiCake>("k");
            container.RegisterType<IProvider, Sensescans>("s");
            container.RegisterType<IProvider, Kissmanga>("ki");
            container.RegisterType<IProvider, Mangafox>("mf");
            container.RegisterType<IProvider, Mangahere>("mh");
            container.RegisterType<IProvider, Mangareader>("mr");
            container.RegisterType<IProvider, Tomochan>("t");
            container.RegisterType<IProvider, Webtoons>("w");
            container.RegisterType<IProvider, YoManga>("y");
            container.RegisterType<IEnumerable<IProvider>, IProvider[]>();
            container.RegisterType<IProviderService, ProviderService>();
            container.RegisterType<IViewModelFactory, ViewModelFactory>();
            container.RegisterType<MainWindowViewModel>();
            container.RegisterType<SettingsViewModel>();
            container.RegisterType<AddMangaViewModel>();
            container.RegisterType<MangaListViewModel>();
            container.RegisterType<PluginsViewModel>();
            container.RegisterType<HistoryViewModel>();
            container.RegisterType<NewMangaViewModel>();
            container.RegisterType<ThemeViewModel>();
            container.RegisterType<ThemeHelper>();
            container.RegisterType<ViewerWindow>();
            var p = container.Resolve<IProviderService>();
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db"))) {
                db.CheckDbVersion(p.Providers);
            }
            else {
                db.CreateDatabase(p.Providers);
            }
            var mainWindow = new MainWindow {
                DataContext = container.Resolve<MainWindowViewModel>()
            };
            var th = container.Resolve<ThemeViewModel>();
            th.SetupTheme();
            await Task.Delay(400);
            mainWindow.ShowDialog();
        }
    }
}