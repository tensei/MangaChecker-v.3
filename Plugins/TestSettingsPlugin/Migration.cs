using System.ComponentModel.Composition;
using MangaChecker.DataTypes.Interfaces;
using TestSettingsPlugin.ViewModels;
using TestSettingsPlugin.Views;

namespace TestSettingsPlugin {
    [Export(typeof(IPlugin))]
    [ExportMetadata("Title", "Migration")]
    [ExportMetadata("Description", "Migration tool for old => new db")]
    [ExportMetadata("Version", "0.1")]
    [ExportMetadata("Author", "Tensei")]
    public class Migration : IPlugin {
        public object SettingsView() {
            return new MainView {DataContext = new MainViewModel()};
        }

        public void Initialize() {
            //throw new NotImplementedException();
        }

        public void Dispose() {
            //throw new NotImplementedException();
        }
    }
}