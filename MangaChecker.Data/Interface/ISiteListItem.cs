namespace MangaChecker.Data.Interface {
    public interface ISiteListItem {
        string Name { get; set; }
        bool Overrideable { get; set; }
        int IsEnabled { get; set; }
    }
}