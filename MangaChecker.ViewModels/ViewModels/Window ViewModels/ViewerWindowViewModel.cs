using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaChecker.Data.Interfaces;
using MangaChecker.Utilities;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels.Window_ViewModels {
    [ImplementPropertyChanged]
    public class ViewerWindowViewModel : IDisposable {
        private readonly ObservableCollection<object> _images = new ObservableCollection<object>();
        private readonly IManga _manga;

        private readonly ObservableCollection<int> _pages = new ObservableCollection<int>();

        private readonly WebParser _webParser = new WebParser();

        private List<object> _imgs;

        private bool _isClosing;

        public ViewerWindowViewModel(IManga manga, IProvider provider) {
            Pages = new ReadOnlyObservableCollection<int>(_pages);
            Images = new ReadOnlyObservableCollection<object>(_images);
            LoadImages(manga, provider).ConfigureAwait(false);
            Title = $"{manga.Name} {manga.Chapter}";
            _manga = manga;
            ChangeModeCommand = new ActionCommand(ChangeMode);
            SaveImagesCommand = new ActionCommand(async () => {
                if (SaveProgress == "Collapsed") {
                    await SaveImagesAsync();
                }
            });
        }

        public ReadOnlyObservableCollection<object> Images { get; }
        public string Title { get; set; }

        public int TargetImages { get; private set; }

        public ICommand ChangeModeCommand { get; }
        public ICommand SaveImagesCommand { get; }
        public ReadOnlyObservableCollection<int> Pages { get; }

        public int SelectedPage { get; set; }

        public int TransIndex { get; set; } = 1;

        public string Mode { get; set; } = "Long Strip";


        public string SaveProgress { get; set; } = "Collapsed";

        public int ProgressValue { get; set; }

        public bool SaveEnabled { get; set; } = true;

        public void Dispose() {
            _isClosing = true;
            _imgs?.Clear();
            _images?.Clear();
            GC.Collect();
        }

        private async Task LoadImages(IManga manga, IProvider provider) {
            var imgs = await provider.GetImagesTaskAsync(manga.Link);
            _imgs = imgs.Item1;
            PageIntList();
            TargetImages = imgs.Item2;
            SelectedPage = 0;
            await Fill().ConfigureAwait(false);
        }

        private async Task Fill() {
            foreach (var image in _imgs) {
                _images.Add(image);
                await Task.Delay(100);
            }
        }

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

        private async Task SaveImagesAsync() {
            if (!SaveEnabled) {
                return;
            }
            SaveEnabled = false;
            var name = Regex.Replace(_manga.Name, "[^0-9a-zA-Z]+", " ").Trim();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", _manga.Site,
                name, _manga.Chapter.ToString(CultureInfo.InvariantCulture));

            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            SaveProgress = "Visible";
            for (var i = 0; i < _images.Count; i++) {
                if (_isClosing) {
                    break;
                }
                var img = _images[i];
                ProgressValue = i + 1;
                if (img is string) {
                    var client = await _webParser.GetHtmlDataAsync((string) img);
                    img = BytesToBitmapImage(client);
                }
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource) img));
                using (var stream = new FileStream($"{Path.Combine(folder, $"{i + 1}.png")}", FileMode.Create)) {
                    encoder.Save(stream);
                }
                await Task.Delay(50);
            }
            SaveProgress = "Collapsed";
            SaveEnabled = true;
        }

        private static BitmapImage BytesToBitmapImage(byte[] bytes) {
            if (bytes == null) {
                return null;
            }
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
    }
}