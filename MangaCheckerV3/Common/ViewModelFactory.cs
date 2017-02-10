using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels;
using MangaChecker.ViewModels.ViewModels;
using MangaCheckerV3.Helpers;

namespace MangaCheckerV3.Common {
    public class ViewModelFactory : IViewModelFactory {
        private readonly ILiteDb _liteDb;
        private readonly IWindowFactory _windowFactory;
        private readonly IProviderService _providerService;
        private readonly IPluginHost _pluginHost;
        private readonly ThemeHelper _themeHelper;
        private readonly Logger _logger;
        public ViewModelFactory(IProviderService providerService, IWindowFactory windowFactory,
            ILiteDb liteDb, ThemeHelper themeHelper, Logger logger, IPluginHost pluginHost) {
            _liteDb = liteDb;
            _providerService = providerService;
            _windowFactory = windowFactory;
            _themeHelper = themeHelper;
            _logger = logger;
            _pluginHost = pluginHost;
        }

        public MangaListViewModel CreateMangaListViewModel => new MangaListViewModel(_providerService, _windowFactory, _liteDb);
        public AddMangaViewModel CreateAddMangaViewModel => new AddMangaViewModel(_providerService, _liteDb);
        public SettingsViewModel CreateSettingsViewModel => new SettingsViewModel(_liteDb);
        public PluginsViewModel CreatePluginsViewModel => new PluginsViewModel(_pluginHost);
        public ThemeViewModel CreateThemeViewModel => new ThemeViewModel(_themeHelper);
        public NewMangaViewModel CreateNewMangaViewModel => new NewMangaViewModel(_windowFactory, _liteDb);
        public HistoryViewModel CreateHistoryViewModel => new HistoryViewModel(_windowFactory, _liteDb);
    }
}
