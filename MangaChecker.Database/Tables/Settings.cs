using System;
using PropertyChanged;

//using SQLite;

namespace MangaChecker.Database.Tables {
    [ImplementPropertyChanged]
    public class Settings {
        //[PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //[NotNull]
        public string Setting { get; set; }

        //[NotNull]
        public string Link { get; set; }

        //[NotNull]
        public DateTime Created { get; set; }

        //[NotNull]
        public int Active { get; set; }

        //[NotNull]
        public bool OpenLinks { get; set; }
    }
}