using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data;
using MangaChecker.Database;
using MangaChecker.Providers;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Providers.Sites;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;
using MangaCheckerV3.Common;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Views.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Ninject;

namespace MangaCheckerV3
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs args)
        {
            //if (!Debugger.IsAttached)
            //	ExceptionHandler.AddGlobalHandlers();
            var kernel = new StandardKernel();
            kernel.Bind<IDbContext>().To<DbContext>().InSingletonScope();
            var db = kernel.Get<DbContext>();

            GlobalVariables.DbContext = db;
            kernel.Bind<INewChapterHelper>().To<NewChapterHelper>();
            kernel.Bind<IWebParser>().To<WebParser>().InSingletonScope();
            kernel.Bind<IWindowFactory>().To<WindowFactory>();
            kernel.Bind<Logger>().ToSelf();

            kernel.Bind<IProvider>().To<Mangastream>().InSingletonScope();
            kernel.Bind<IProvider>().To<Mangadex>().InSingletonScope();
            kernel.Bind<IProvider>().To<Crunchyroll>().InSingletonScope();
            kernel.Bind<IProvider>().To<GameOfScanlation>().InSingletonScope();
            kernel.Bind<IProvider>().To<HeyManga>().InSingletonScope();
            kernel.Bind<IProvider>().To<Jaiminisbox>().InSingletonScope();
            kernel.Bind<IProvider>().To<KireiCake>().InSingletonScope();
            kernel.Bind<IProvider>().To<Sensescans>().InSingletonScope();
            kernel.Bind<IProvider>().To<Kissmanga>().InSingletonScope();
            kernel.Bind<IProvider>().To<Mangafox>().InSingletonScope();
            kernel.Bind<IProvider>().To<Mangahere>().InSingletonScope();
            kernel.Bind<IProvider>().To<Mangareader>().InSingletonScope();
            kernel.Bind<IProvider>().To<Tomochan>().InSingletonScope();
            kernel.Bind<IProvider>().To<Webtoons>().InSingletonScope();
            kernel.Bind<IProvider>().To<Mangazuki>().InSingletonScope();
            
            kernel.Bind<SwatchesProvider>().ToSelf().InSingletonScope();
            kernel.Bind<PaletteHelper>().ToSelf().InSingletonScope();

            kernel.Bind<IProviderService>().To<ProviderService>().InSingletonScope();
            kernel.Bind<IViewModelFactory>().To<ViewModelFactory>().InSingletonScope();
            kernel.Bind<IThemeHelper>().To<ThemeHelper>().InSingletonScope();
            kernel.Bind<ILinkParser>().To<LinkParser>().InSingletonScope();

            kernel.Bind<ThemeViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<AddMangaViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<MangaListViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<HistoryViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<NewMangaViewModel>().ToSelf().InSingletonScope();

            kernel.Bind<ViewerWindow>().ToSelf();
            kernel.Bind<MainWindow>().ToSelf();

            var collection = kernel.GetAll<IProvider>();
            var pdict = collection.ToDictionary(k => k.DbName, v => v.LinktoSite);
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db")))
            {
                db.CheckDbVersion(pdict);
            }
            else
            {
                db.CreateDatabase(pdict);
            }
            kernel.Get<MainWindow>().Show();
        }
    }
}