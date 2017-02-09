namespace MangaChecker.Data.Interfaces {
    public interface ISiteListItem {
        string Name { get; set; }
        bool Overrideable { get; set; }
        int IsEnabled { get; set; }
    }
}