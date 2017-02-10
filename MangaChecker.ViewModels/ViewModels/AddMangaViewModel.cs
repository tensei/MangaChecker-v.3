using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.ViewModels.ViewModels.Adding_ViewModels;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels {
    [ImplementPropertyChanged]
    public class AddMangaViewModel {
        public AddMangaViewModel(IProviderSet providerService, ILiteDb liteDb) {
            NormalDataContext = new NormalViewModel();
            NormalCommand = new ActionCommand(() => {
                if (TranIndex == 0) {
                    return;
                }
                TranIndex = 0;
                NormalDataContext = new NormalViewModel();
            });
            AdvancedCommand = new ActionCommand(() => {
                if (TranIndex == 1) {
                    return;
                }
                TranIndex = 1;
                AdvancedDataContext = new AdvancedViewModel(providerService, liteDb);
            });
        }

        public int TranIndex { get; set; }

        public NormalViewModel NormalDataContext { get; set; }

        public AdvancedViewModel AdvancedDataContext { get; set; }

        public ICommand NormalCommand { get; }

        public ICommand AdvancedCommand { get; }
    }
}