using System;

namespace MangaChecker.Data.Interface {
    public interface IRssItem {
        string Title { get; set; }
        string Author { get; set; }
        string Link { get; set; }
        string Guid { get; set; }
        string Category { get; set; }
        string Description { get; set; }
        DateTime PubDate { get; set; }
    }
}