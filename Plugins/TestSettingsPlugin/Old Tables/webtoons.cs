using SQLite;

namespace TestSettingsPlugin.Old_Tables {
    public class webtoons {
        [PrimaryKey]
        [AutoIncrement]
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