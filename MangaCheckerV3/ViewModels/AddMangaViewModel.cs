using System.Windows.Input;
using MangaCheckerV3.ViewModels.Adding_ViewModels;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class AddMangaViewModel {
        public AddMangaViewModel() {
            NormalDataContext = new NormalViewModel();
            NormalCommand = new ActionCommand(() => {
                if (TranIndex == 0) return;
                TranIndex = 0;
                NormalDataContext = new NormalViewModel();
            });
            AdvancedCommand = new ActionCommand(() => {
                if (TranIndex == 1) return;
                TranIndex = 1;
                AdvancedDataContext = new AdvancedViewModel();
            });
        }

        public int TranIndex { get; set; }

        public NormalViewModel NormalDataContext { get; set; }

        public AdvancedViewModel AdvancedDataContext { get; set; }

        public ICommand NormalCommand { get; }

        public ICommand AdvancedCommand { get; }
    }
}