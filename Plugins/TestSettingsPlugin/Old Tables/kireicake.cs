using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TestSettingsPlugin.Old_Tables {
	public class kireicake {
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
