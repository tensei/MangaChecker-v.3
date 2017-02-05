using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using MangaChecker.Database.Tables;
using MangaChecker.Utilities;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Window_ViewModels {
    [ImplementPropertyChanged]
    public class ViewerWindowViewModel : IDisposable{
        private readonly ObservableCollection<object> _images = new ObservableCollection<object>();
        public ReadOnlyObservableCollection<object> Images { get; }

        private readonly ObservableCollection<object> _singleimage = new ObservableCollection<object>();
        public ReadOnlyObservableCollection<object> SingleImages { get; }
        private int _currentImg;
        private int _selectedPage;

        public string Title { get; set; }

        public int TargetImages { get; set; }

        private List<object> imgs;
        private Manga Manga;
        public ViewerWindowViewModel(List<object> images, Manga manga, int pages) {
            Title = $"{manga.Name} {manga.Chapter}";
            Manga = manga;
            imgs = images;
            TargetImages = pages;
            NextPageCommand = new ActionCommand(NextPage);
            PrevPageCommand = new ActionCommand(PrevPage);
            ChangeModeCommand = new ActionCommand(ChangeMode);
            SaveImagesCommand = new ActionCommand(async () => {
                if (SaveProgress == Visibility.Collapsed)
                    await SaveImagesAsync();
            });
            Images = new ReadOnlyObservableCollection<object>(_images);
            SingleImages = new ReadOnlyObservableCollection<object>(_singleimage);
            _singleimage.Add(imgs[0]);
            SelectedPage = 1;
        }

        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        
        public ICommand ChangeModeCommand { get; }
        public ICommand SaveImagesCommand { get; }

        public List<int> Pages => PageIntList();

        private async Task Fill() {
            if(_images.Count > 0) _images.Clear();
            foreach (var image in imgs) {
                if(TransIndex == 0) break;
                _images.Add(image);
                await Task.Delay(100);
            }
        }

        public int SelectedPage {
            get { return _selectedPage; }
            set {
                _selectedPage = value;
                Jumpto(value);
            }
        }

        public int TransIndex { get; set; }

        public string Mode { get; set; } = "Singe Page";

        private void ChangeMode() {
            if (TransIndex == 0) {
                TransIndex = 1;
                Mode = "Long Strip";
                Fill().ConfigureAwait(false);
                return;
            }
            if (_images.Count > 0) _images.Clear();
            TransIndex = 0;
            Mode = "Singe Page";
        }

        private List<int> PageIntList() {
            var p = new List<int>();
            for (var i = 0; i < imgs.Count; i++) {
                p.Add(i+1);
            }
            return p;
        }

        private void NextPage() {
            if (_currentImg == imgs.Count-1) {
                return;
            }
            _currentImg++;
            SelectedPage = _currentImg + 1;
            _singleimage.Add(imgs[_currentImg]);
        }
        private void PrevPage() {
            if (_currentImg == 0) {
                return;
            }
            _currentImg--;
            SelectedPage = _currentImg+1;
            _singleimage.Add(imgs[_currentImg]);
        }

        private void Jumpto(int page) {
            _currentImg = page-1;
            _singleimage.Clear();
            _singleimage.Add(imgs[_currentImg]);
        }

        public Visibility SaveProgress { get; set; } = Visibility.Collapsed;

        public int ProgressValue { get; set; }

        public bool SaveEnabled { get; set; } = true;

        private bool _isClosing;

        private readonly WebParser _webParser = new WebParser();
        private async Task SaveImagesAsync() {
            if (!SaveEnabled) return;
            SaveEnabled = false;
            var name = Regex.Replace(Manga.Name, "[^0-9a-zA-Z]+", " ").Trim();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", Manga.Site,
                name, Manga.Chapter.ToString(CultureInfo.InvariantCulture));

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            SaveProgress = Visibility.Visible;
            for (var i = 0; i < _images.Count; i++) {
                if (_isClosing)
                    break;
                var img = _images[i];
                ProgressValue = i + 1;
                if (img is string) {
                    var client = await _webParser.GetHtmlDataAsync((string)img);
                    img = BytesToBitmapImage(client);
                }
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)img));
                using (var stream = new FileStream($"{Path.Combine(folder, $"{i + 1}.png")}", FileMode.Create)) {
                    encoder.Save(stream);
                }
            }
            SaveProgress = Visibility.Collapsed;
            SaveEnabled = true;
        }

        private static BitmapImage BytesToBitmapImage(byte[] bytes) {
            if (bytes == null)
                return null;
            using (var mem = new MemoryStream(bytes, 0, bytes.Length)) {
                mem.Position = 0;
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        public void Dispose() {
            _isClosing = true;
            imgs.Clear();
            _images.Clear();
            _singleimage.Clear();
            GC.Collect();
        }
    }
}