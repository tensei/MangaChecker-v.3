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
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;
using MangaCheckerV3.Common;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Window_ViewModels {
    [ImplementPropertyChanged]
    public class ViewerWindowViewModel : IDisposable{
        private readonly ObservableCollection<object> _images = new ObservableCollection<object>();
        public ReadOnlyObservableCollection<object> Images { get; }
        
        public string Title { get; set; }

        public int TargetImages { get; private set; }

        private List<object> _imgs;
        private readonly Manga _manga;
        public ViewerWindowViewModel(Manga manga, ISite provider) {
            Pages = new ReadOnlyObservableCollection<int>(_pages);
            LoadImages(manga, provider).ConfigureAwait(false);
            Title = $"{manga.Name} {manga.Chapter}";
            _manga = manga;
            ChangeModeCommand = new ActionCommand(ChangeMode);
            SaveImagesCommand = new ActionCommand(async () => {
                if (SaveProgress == Visibility.Collapsed)
                    await SaveImagesAsync();
            });
            Images = new ReadOnlyObservableCollection<object>(_images);
        }

        private async Task LoadImages(Manga manga, ISite provider) {
            var imgs = await provider.GetImagesTaskAsync(manga.Link);
            _imgs = imgs.Item1;
            PageIntList();
            TargetImages = imgs.Item2;
            SelectedPage = 0;
            await Fill().ConfigureAwait(false);
        }
        
        public ICommand ChangeModeCommand { get; }
        public ICommand SaveImagesCommand { get; }

        private readonly ObservableCollection<int> _pages = new ObservableCollection<int>();
        public ReadOnlyObservableCollection<int> Pages { get; }

        private async Task Fill() {
            foreach (var image in _imgs) {
                _images.Add(image);
                await Task.Delay(100);
            }
        }

        public int SelectedPage { get; set; }

        public int TransIndex { get; set; } = 1;

        public string Mode { get; set; } = "Singe Page";

        private void ChangeMode() {
            if (TransIndex == 1) {
                TransIndex = 0;
                Mode = "Singe Page";
                return;
            }
            TransIndex = 1;
            Mode = "Long Strip";
        }

        private void PageIntList() {
            for (var i = 0; i < _imgs.Count; i++) {
                _pages.Add(i);
            }
        }
        
        
        public Visibility SaveProgress { get; set; } = Visibility.Collapsed;

        public int ProgressValue { get; set; }

        public bool SaveEnabled { get; set; } = true;

        private bool _isClosing;

        private readonly WebParser _webParser = new WebParser();
        private async Task SaveImagesAsync() {
            if (!SaveEnabled) return;
            SaveEnabled = false;
            var name = Regex.Replace(_manga.Name, "[^0-9a-zA-Z]+", " ").Trim();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", _manga.Site,
                name, _manga.Chapter.ToString(CultureInfo.InvariantCulture));

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
            _imgs.Clear();
            _images.Clear();
            GC.Collect();
        }
    }
}