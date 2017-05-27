using System.ComponentModel.Composition;
using DiscordMC.ViewModels;
using DiscordMC.Views;
using MangaChecker.Data.Interfaces;

namespace DiscordMC {
    [Export(typeof(IPlugin)), ExportMetadata("Title", "DiscordMC"),
     ExportMetadata("Description", "Announcing new releases"), ExportMetadata("Version", "0.1"),
     ExportMetadata("Author", "Tensei")]
    public class DiscordMc : IPlugin {
        private MainView _mainView;
        private MainViewModel _mainViewModel;

        public async void Dispose() {
            var closeAsync = _mainViewModel?.CloseAsync();
            if (closeAsync != null) {
                await closeAsync;
            }
        }

        public object SettingsView() {
            _mainViewModel = new MainViewModel();
            _mainView = new MainView {DataContext = _mainViewModel};
            return _mainView;
        }

        public void Initialize() {
        }
    }
}