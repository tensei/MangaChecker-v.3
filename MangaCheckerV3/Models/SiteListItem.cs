using PropertyChanged;

namespace MangaCheckerV3.Models {
    [ImplementPropertyChanged]
    public class SiteListItem {
        public string Name { get; set; }
        public bool Overrideable { get; set; } = true;
        public int IsEnabled { get; set; }
    }
}
