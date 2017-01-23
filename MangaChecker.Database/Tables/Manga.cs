using System;
using PropertyChanged;
using SQLite;

namespace MangaChecker.Database.Tables {
	[ImplementPropertyChanged]
	public class Manga {
		[PrimaryKey]
		[AutoIncrement]
		[Unique]
		public int Id { get; set; }

		[NotNull]
		public string Name { get; set; }

		[NotNull]
		public float Chapter { get; set; }

		[NotNull]
		public string Site { get; set; }
		
		public string Link { get; set; }

		public string Rss { get; set; }
		
		public string SpecialLink { get; set; }

		[NotNull]
		public DateTime Added { get; set; } = DateTime.Now;

		[NotNull]
		public DateTime Updated { get; set; } = DateTime.Now;

		[Ignore]
		public string DaysAgo => DaysSinceUpdate();
		private string DaysSinceUpdate() {
			var dateNow = DateTime.Now;
			var diff = dateNow - Updated;
			if (diff.Days < 0) return "Unknown";
			return diff.Days == 0 ? "Today" : $"{diff.Days} day(s) ago";
		}
	}
}