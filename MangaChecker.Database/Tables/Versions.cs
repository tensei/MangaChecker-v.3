using SQLite;

namespace MangaCheckerV3.SQLite.Tables {
	public class Versions {
		[PrimaryKey]
		public string Name { get; set; }
		public string Version { get; set; }

	}
}