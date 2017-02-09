using System;
using MangaChecker.Data.Interface;

namespace MangaChecker.Data.Model {
    public class RssItem : IRssItem {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
    }
}