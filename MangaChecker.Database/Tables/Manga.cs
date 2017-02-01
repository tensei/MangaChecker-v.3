using System;
using System.Collections.Generic;
using LiteDB;
using MangaChecker.Database.Enums;
using PropertyChanged;

namespace MangaChecker.Database.Tables {
    [ImplementPropertyChanged]
    public class Manga {
        [BsonId]
        public int MangaId { get; set; }

        public string Name { get; set; }

        public float Chapter { get; set; }

        public object Newest { get; set; }

        public bool New { get; set; } = false;

        public List<string> OtherChapters { get; set; } = new List<string>();

        public string Site { get; set; }

        public string Link { get; set; }

        public string Rss { get; set; }

        public string SpecialLink { get; set; }

        public string Thumbnail { get; set; }

        public DateTime Added { get; set; } = DateTime.Now;

        public List<Genre> Genres { get; set; } = new List<Genre>();

        public DateTime Updated { get; set; } = DateTime.Now;

        [BsonIgnore]
        public string DaysAgo => DaysSinceUpdate();

        private string DaysSinceUpdate() {
            var dateNow = DateTime.Now;
            var diff = dateNow - Updated;
            if (diff.Days < 0) return "Unknown";
            return diff.Days == 0 ? "Today" : $"{diff.Days} day(s) ago";
        }
    }
}