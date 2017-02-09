using System;
using MangaChecker.Data.Interfaces;
using PropertyChanged;

//using SQLite;

namespace MangaChecker.Data.Models {
    [ImplementPropertyChanged]
    public class Settings : ISettings {
        public int Id { get; set; }

        public string Setting { get; set; }

        public string Link { get; set; }

        public DateTime Created { get; set; }

        public int Active { get; set; }

        public bool OpenLinks { get; set; }
    }
}