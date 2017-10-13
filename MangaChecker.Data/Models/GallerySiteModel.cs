using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MangaChecker.Data.Models
{
    public class GallerySiteModel : INotifyPropertyChanged
    {
        public string Site { get; set; }
        public string Path { get; set; }
        public ObservableCollection<GalleryManga> Mangas { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class GalleryManga : INotifyPropertyChanged
    {
        public string Site { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
        public ObservableCollection<GalleryChapter> Chapters { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class GalleryChapter : INotifyPropertyChanged
    {
        public string Site { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}