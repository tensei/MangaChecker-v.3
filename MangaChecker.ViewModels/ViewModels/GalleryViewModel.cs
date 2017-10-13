using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using MangaChecker.Data.Models;

namespace MangaChecker.ViewModels.ViewModels
{
    public class GalleryViewModel : INotifyPropertyChanged
    {
        public GalleryViewModel()
        {
            Sites = new ObservableCollection<GallerySiteModel>();
            Load();
        }

        public ObservableCollection<GallerySiteModel> Sites { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Load()
        {
            var saves = Path.Combine(Directory.GetCurrentDirectory(), "Saves");
            if (!Directory.Exists(saves))
            {
                return;
            }
            var sites = Directory.GetDirectories(saves);
            GetSites(sites);
        }

        private void GetSites(string[] paths)
        {
            foreach (var site in paths)
            {
                var sitename = Path.GetFileName(site);
                var galleryitem = new GallerySiteModel
                {
                    Site = sitename,
                    Path = site,
                    Mangas = new ObservableCollection<GalleryManga>()
                };
                GetMangas(site, sitename, galleryitem);
                Sites.Add(galleryitem);
            }
        }

        private void GetMangas(string path, string sitename, GallerySiteModel galleryitem)
        {
            var mangas = Directory.GetDirectories(path);
            foreach (var manga in mangas)
            {
                var manganame = Path.GetFileName(manga);
                var gallerymangaitem = new GalleryManga
                {
                    Name = manganame,
                    Site = sitename,
                    Path = manga,
                    Created = new DirectoryInfo(manga).CreationTime,
                    Chapters = new ObservableCollection<GalleryChapter>()
                };
                GetChapters(manga, sitename, manganame, gallerymangaitem);
                galleryitem.Mangas.Add(gallerymangaitem);
            }
        }

        private void GetChapters(string path, string sitename, string manganame, GalleryManga gallerymangaitem)
        {
            var chapters = Directory.GetDirectories(path);
            foreach (var chapter in chapters)
            {
                var images = Directory.GetFiles(chapter);
                var gallerychapteritem = new GalleryChapter
                {
                    Number = Path.GetFileName(chapter),
                    Site = sitename,
                    Name = manganame,
                    Path = chapter,
                    Created = new DirectoryInfo(chapter).CreationTime,
                    Images = new ObservableCollection<string>()
                };
                images.ToList().ForEach(gallerychapteritem.Images.Add);
                gallerymangaitem.Chapters.Add(gallerychapteritem);
                gallerymangaitem.Chapters?.OrderBy(c => float.Parse(c.Number, CultureInfo.InvariantCulture));
            }
        }
    }
}