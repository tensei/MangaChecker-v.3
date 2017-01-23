using SQLite;

namespace MangaChecker.Database.Tables {
	public class Versions {
		[PrimaryKey]
		public string Name { get; set; }
		public string Version { get; set; }

	}
}