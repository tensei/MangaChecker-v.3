﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MangaChecker.Data.Interfaces;
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
using Microsoft.Practices.Unity;
using Unity;

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

            var container = new UnityContainer();
            container.RegisterInstance<IDbContext>(new DbContext());

            var db = container.Resolve<IDbContext>();
            GlobalVariables.DbContext = db;
            container.RegisterType<INewChapterHelper, NewChapterHelper>();
            container.RegisterType<IWebParser, WebParser>();
            container.RegisterType<IWindowFactory, WindowFactory>();
            container.RegisterType<Logger>();

            container.RegisterInstance<IPluginHost>(new PluginHost());

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
            container.RegisterType<IProvider, Mangazuki>("y");

            container.RegisterType<IEnumerable<IProvider>, IProvider[]>();

            var collection = container.Resolve<IProvider[]>();
            container.RegisterInstance<IProviderSet>(new ProviderSet(collection));

            container.RegisterInstance(new SwatchesProvider());
            container.RegisterInstance(new PaletteHelper());

            container.RegisterType<IProviderService, ProviderService>();
            container.RegisterType<IViewModelFactory, ViewModelFactory>();
            container.RegisterType<IThemeHelper, ThemeHelper>();
            container.RegisterType<ILinkParser, LinkParser>();

            container.RegisterType<ThemeViewModel>();
            container.RegisterType<MainWindowViewModel>();
            container.RegisterType<SettingsViewModel>();
            container.RegisterType<AddMangaViewModel>();
            container.RegisterType<MangaListViewModel>();
            container.RegisterType<PluginsViewModel>();
            container.RegisterType<HistoryViewModel>();
            container.RegisterType<NewMangaViewModel>();

            container.RegisterType<ViewerWindow>();
            container.RegisterType<MainWindow>();

            var pdict = collection.ToDictionary(k => k.DbName, v => v.LinktoSite);
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db")))
            {
                db.CheckDbVersion(pdict);
            }
            else
            {
                db.CreateDatabase(pdict);
            }

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}