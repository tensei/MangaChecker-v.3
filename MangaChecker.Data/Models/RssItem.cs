using System;
using MangaChecker.Data.Interfaces;

namespace MangaChecker.Data.Models
{
    public class RssItem : IRssItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
    }
}