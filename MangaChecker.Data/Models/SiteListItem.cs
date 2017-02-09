using MangaChecker.Data.Interfaces;
using PropertyChanged;

namespace MangaChecker.Data.Models {
    [ImplementPropertyChanged]
    public class SiteListItem : ISiteListItem {
        public string Name { get; set; }
        public bool Overrideable { get; set; } = true;
        public int IsEnabled { get; set; }
    }
}