using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Data.Interface;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Common {
    public class ViewModelFactory : IViewModelFactory {
        private readonly ILiteDb _liteDb;
        private readonly Utilities _utilities;
        private readonly IProviderService _providerService;
        private readonly ThemeHelper _themeHelper;
        public ViewModelFactory(IProviderService providerService, Utilities utilities, ILiteDb liteDb, ThemeHelper themeHelper) {
            _liteDb = liteDb;
            _providerService = providerService;
            _utilities = utilities;
            _themeHelper = themeHelper;
        }

        public MangaListViewModel CreateMangaListViewModel => new MangaListViewModel(_providerService, _utilities, _liteDb);
        public AddMangaViewModel CreateAddMangaViewModel => new AddMangaViewModel(_providerService, _liteDb);
        public SettingsViewModel CreateSettingsViewModel => new SettingsViewModel(_liteDb);
        public PluginsViewModel CreatePluginsViewModel => new PluginsViewModel();
        public ThemeViewModel CreateThemeViewModel => new ThemeViewModel(_themeHelper);
        public NewMangaViewModel CreateNewMangaViewModel => new NewMangaViewModel(_utilities, _liteDb);
        public HistoryViewModel CreateHistoryViewModel => new HistoryViewModel(_utilities, _liteDb);
    }
}
