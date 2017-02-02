using System;
using PropertyChanged;

//using SQLite;

namespace MangaChecker.Database.Tables {
    [ImplementPropertyChanged]
    public class Settings {
        public int Id { get; set; }

        public string Setting { get; set; }

        public string Link { get; set; }

        public DateTime Created { get; set; }

        public int Active { get; set; }

        public bool OpenLinks { get; set; }
    }
}