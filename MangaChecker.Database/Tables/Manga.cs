using System;
using System.Collections.Generic;
using LiteDB;
using PropertyChanged;

namespace MangaChecker.Database.Tables {
    [ImplementPropertyChanged]
	public class Manga {
        [BsonId]
		public int MangaId { get; set; }

		public string Name { get; set; }
        
		public float Chapter { get; set; }

        public List<string> OtherChapters { get; set; }
        
		public string Site { get; set; }
		
		public string Link { get; set; }

		public string Rss { get; set; }
		
		public string SpecialLink { get; set; }
        
		public DateTime Added { get; set; } = DateTime.Now;
        
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