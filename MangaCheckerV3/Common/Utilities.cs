using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Utilities;
using MangaCheckerV3.ViewModels.Window_ViewModels;
using MangaCheckerV3.Views.Windows;

namespace MangaCheckerV3.Common {
    public class Utilities {
        private ViewerWindow _viewerWindow;
        private Logger _logger;
        private readonly IProviderService _providerService;

        public Utilities(ViewerWindow viewerWindow, IProviderService providerService, Logger logger) {
            _viewerWindow = viewerWindow;
            _providerService = providerService;
        }
        public void OpenViewer(Manga manga) {
            var provider = _providerService.Providers.Find(p => p.DbName == manga.Site);
            if (!provider.ViewEnabled) {
                try {
                    Process.Start(manga.Link);
                }
                catch (Exception e) {
                    _logger.Log.Error($"{manga.Link}\n{e}");
                }
                return;
            }
            _viewerWindow = new ViewerWindow {
                DataContext = new ViewerWindowViewModel(manga, provider),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            _viewerWindow.Show();
        }
    }
}