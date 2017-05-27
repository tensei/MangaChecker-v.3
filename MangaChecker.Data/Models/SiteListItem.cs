using System.ComponentModel;
using MangaChecker.Data.Interfaces;

namespace MangaChecker.Data.Models {
    public class SiteListItem : ISiteListItem, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public bool Overrideable { get; set; } = true;
        public int IsEnabled { get; set; }
    }
}