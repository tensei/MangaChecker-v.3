using MangaChecker.Data.Interface;
using PropertyChanged;

namespace MangaChecker.Data.Model {
    [ImplementPropertyChanged]
    public class SiteListItem : ISiteListItem {
        public string Name { get; set; }
        public bool Overrideable { get; set; } = true;
        public int IsEnabled { get; set; }
    }
}