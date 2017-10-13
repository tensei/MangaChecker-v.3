using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;
using MangaCheckerV3.Helpers;

namespace MangaCheckerV3.Common
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;
        private readonly IPluginHost _pluginHost;
        private readonly IProviderSet _providerSet;
        private readonly ThemeHelper _themeHelper;
        private readonly IWindowFactory _windowFactory;

        public ViewModelFactory(IProviderSet providerSet, IWindowFactory windowFactory,
            IDbContext dbContext, ThemeHelper themeHelper, Logger logger, IPluginHost pluginHost)
        {
            _dbContext = dbContext;
            _providerSet = providerSet;
            _windowFactory = windowFactory;
            _themeHelper = themeHelper;
            _logger = logger;
            _pluginHost = pluginHost;
        }

        public MangaListViewModel CreateMangaListViewModel => new MangaListViewModel(_providerSet, _windowFactory,
            _dbContext);

        public AddMangaViewModel CreateAddMangaViewModel => new AddMangaViewModel(_providerSet, _dbContext);
        public SettingsViewModel CreateSettingsViewModel => new SettingsViewModel(_dbContext);
        public PluginsViewModel CreatePluginsViewModel => new PluginsViewModel(_pluginHost);
        public ThemeViewModel CreateThemeViewModel => new ThemeViewModel(_themeHelper);
        public NewMangaViewModel CreateNewMangaViewModel => new NewMangaViewModel(_windowFactory, _dbContext);
        public HistoryViewModel CreateHistoryViewModel => new HistoryViewModel(_windowFactory, _dbContext);
        public GalleryViewModel CreateGalleryViewModel => new GalleryViewModel();
    }
}