using System;

namespace MangaChecker.Parser {
    public class RssItemObject {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
    }
}
