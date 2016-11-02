using System;
using SQLite;

namespace TestSettingsPlugin.New_Tables {
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
	}
}