using System;
using System.ComponentModel;
using MangaChecker.Data.Interfaces;

//using SQLite;

namespace MangaChecker.Data.Models {
    public class Settings : ISettings, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public string Setting { get; set; }

        public string Link { get; set; }

        public DateTime Created { get; set; }

        public int Active { get; set; }

        public bool OpenLinks { get; set; }
    }
}