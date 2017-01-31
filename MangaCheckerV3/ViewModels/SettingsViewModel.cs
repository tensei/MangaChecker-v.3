using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class SettingsViewModel {
        private readonly ObservableCollection<Settings> _settings = new ObservableCollection<Settings>();

        public SettingsViewModel() {
            SaveCommand = new ActionCommand(() => {
                Task.Run(() => {
                    var set = _settings.ToList();
                    set.Add(RefreshTime);
                    set.Add(OpenLinks);
                    set.Add(BatotoRss);
                    Database.SaveSettings(set);
                }).ConfigureAwait(false);
            });
            ToggleActive = new ActionCommand(s => Toggle((Settings) s));
            Settings = new ReadOnlyObservableCollection<Settings>(_settings);
            var settings = Database.GetAllSettings();
            foreach (var s in settings) {
                if (s.Setting.ToLower().StartsWith("refres")) RefreshTime = s;
                if (s.Setting.ToLower().StartsWith("open")) OpenLinks = s;
                if (s.Setting.ToLower().StartsWith("batoto rss")) BatotoRss = s;
            }
            settings.RemoveAll(s => s.Setting.ToLower().StartsWith("refres")
                                    || s.Setting.ToLower().StartsWith("backlog")
                                    || s.Setting.ToLower().StartsWith("batoto rss")
                                    || s.Setting.ToLower().StartsWith("open "));
            foreach (var setting in settings) _settings.Add(setting);
        }

        public Settings RefreshTime { get; set; }
        public Settings OpenLinks { get; set; }
        public Settings BatotoRss { get; set; }
        public ReadOnlyObservableCollection<Settings> Settings { get; }

        public ICommand SaveCommand { get; }
        public ICommand ToggleActive { get; }

        private void Toggle(Settings settings) {
            if (settings.Active == 1) {
                settings.Active = 0;
                return;
            }
            settings.Active = 1;
        }
    }
}