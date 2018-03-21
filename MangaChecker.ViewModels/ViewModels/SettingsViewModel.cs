using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;

namespace MangaChecker.ViewModels.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IDbContext _dbContext;
        private readonly ObservableCollection<Settings> _settings = new ObservableCollection<Settings>();

        public SettingsViewModel(IDbContext dbContext)
        {
            _dbContext = dbContext;
            //SaveCommand = new ActionCommand(SaveAll);
            ToggleActive = new ActionCommand(s => Toggle((Settings) s));
            Settings = new ReadOnlyObservableCollection<Settings>(_settings);
            var settings = dbContext.GetAllSettings();
            foreach (var s in settings)
            {
                if (s.Setting.ToLower().StartsWith("refres"))
                {
                    RefreshTime = s;
                    RefreshTime.PropertyChanged += Setting_PropertyChanged;
                }
                if (s.Setting.ToLower().StartsWith("open"))
                {
                    OpenLinks = s;
                    OpenLinks.PropertyChanged += Setting_PropertyChanged;
                }
                if (s.Setting.ToLower().StartsWith("mangadex rss"))
                {
                    MangadexRss = s;
                    MangadexRss.PropertyChanged += Setting_PropertyChanged;
                }
            }
            settings.RemoveAll(s => s.Setting.ToLower().StartsWith("refres")
                                    || s.Setting.ToLower().StartsWith("backlog")
                                    || s.Setting.ToLower().StartsWith("mangadex rss")
                                    || s.Setting.ToLower().StartsWith("open "));
            foreach (var setting in settings)
            {
                _settings.Add(setting);
                setting.PropertyChanged += Setting_PropertyChanged;
            }
        }

        private readonly object _saveLock = new object();
        private void Setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            lock (_saveLock)
            {
                _dbContext.SaveSetting((Settings)sender);
            }
        }
        

        public Settings RefreshTime { get; set; }
        public Settings OpenLinks { get; set; }
        public Settings MangadexRss { get; set; }
        public ReadOnlyObservableCollection<Settings> Settings { get; }
        
        public ICommand ToggleActive { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Toggle(Settings settings)
        {
            if (settings.Active == 1)
            {
                settings.Active = 0;
                return;
            }
            settings.Active = 1;
        }
    }
}