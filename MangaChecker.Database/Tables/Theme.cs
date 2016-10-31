using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MangaCheckerV3.SQLite.Tables {
	public class Theme {
		[PrimaryKey]
		public string Name { get; set; }
		public string Color { get; set; }

	}
}
