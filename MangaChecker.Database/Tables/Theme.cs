using SQLite;

namespace MangaChecker.Database.Tables {
	public class Theme {
		[PrimaryKey]
		public string Name { get; set; }
		public string Color { get; set; }

	}
}
