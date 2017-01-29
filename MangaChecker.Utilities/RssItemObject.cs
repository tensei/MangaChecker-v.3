using System;

namespace MangaChecker.Utilities {
    public class RssItemObject {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
    }
}