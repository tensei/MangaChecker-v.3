using SQLite;

namespace DBMigration.Old_Tables {
    public class mangahere {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [NotNull]
        public string name { get; set; }

        [NotNull]
        public string chapter { get; set; }

        [NotNull]
        public string last_update { get; set; }

        public string link { get; set; }
        public string rss_url { get; set; }
    }
}