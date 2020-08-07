using System.Collections.Generic;
using System.Linq;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;
using MangaChecker.ViewModels.Interfaces;
using MangaChecker.ViewModels.ViewModels;

namespace MangaCheckerV3.Common
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;
        private readonly List<IProvider> _providerSet;
        private readonly IWindowFactory _windowFactory;

        public ViewModelFactory(IEnumerable<IProvider> providerSet, IWindowFactory windowFactory,
            IDbContext dbContext, Logger logger)
        {
            _dbContext = dbContext;
            _providerSet = providerSet.ToList();
            _windowFactory = windowFactory;
            _logger = logger;
        }

        public MangaListViewModel CreateMangaListViewModel => new MangaListViewModel(_providerSet, _windowFactory,
            _dbContext);

        public AddMangaViewModel CreateAddMangaViewModel => new AddMangaViewModel(_providerSet, _dbContext);
        public SettingsViewModel CreateSettingsViewModel => new SettingsViewModel(_dbContext);
        public NewMangaViewModel CreateNewMangaViewModel => new NewMangaViewModel(_windowFactory, _dbContext);
        public HistoryViewModel CreateHistoryViewModel => new HistoryViewModel(_windowFactory, _dbContext);
        public GalleryViewModel CreateGalleryViewModel => new GalleryViewModel();
    }
}