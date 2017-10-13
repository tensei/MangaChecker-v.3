using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;

namespace MangaChecker.ViewModels.ViewModels.Window_ViewModels
{
    public class ViewerWindowViewModel : INotifyPropertyChanged
    {
        private readonly Action _closeAction;
        private readonly ObservableCollection<object> _images = new ObservableCollection<object>();
        private readonly IManga _manga;

        private readonly ObservableCollection<int> _pages = new ObservableCollection<int>();

        private readonly WebParser _webParser = new WebParser();

        private List<object> _imgs;

        private bool _isClosing;

        public ViewerWindowViewModel(IManga manga, IProvider provider, Action close, bool saveEnabled = true)
        {
            SaveEnabled = saveEnabled;
            _closeAction = close;
            Pages = new ReadOnlyObservableCollection<int>(_pages);
            Images = new ReadOnlyObservableCollection<object>(_images);
            LoadImages(manga, provider).ConfigureAwait(false);
            Title = $"{manga.Name} {manga.Chapter}";
            _manga = manga;
            ChangeModeCommand = new ActionCommand(ChangeMode);
            SaveImagesCommand = new ActionCommand(async () =>
            {
                if (SaveProgress == "Collapsed")
                {
                    await SaveImagesAsync();
                }
            });
        }

        public ViewerWindowViewModel(GalleryChapter chapter)
        {
            SaveEnabled = false;
            Pages = new ReadOnlyObservableCollection<int>(_pages);
            Images = new ReadOnlyObservableCollection<object>(_images);
            foreach (var image in chapter.Path)
            {
            }
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

        public bool SaveEnabled { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            _isClosing = true;
            _imgs?.Clear();
            _images?.Clear();
            GC.Collect();
        }

        private async Task LoadImages(IManga manga, IProvider provider)
        {
            var imgs = await provider.GetImagesTaskAsync(manga.Link);
            if (imgs.Item1.Count <= 0)
            {
                _closeAction.Invoke();
            }
            _imgs = imgs.Item1;
            PageIntList();
            TargetImages = imgs.Item2;
            SelectedPage = 0;
            await Fill().ConfigureAwait(false);
        }

        private async Task Fill()
        {
            foreach (var image in _imgs)
            {
                _images.Add(image);
                await Task.Delay(100);
            }
        }

        private void ChangeMode()
        {
            if (TransIndex == 1)
            {
                TransIndex = 0;
                Mode = "Singe Page";
                return;
            }
            TransIndex = 1;
            Mode = "Long Strip";
        }

        private void PageIntList()
        {
            for (var i = 0; i < _imgs.Count; i++)
            {
                _pages.Add(i);
            }
        }

        private async Task SaveImagesAsync()
        {
            if (!SaveEnabled)
            {
                return;
            }
            SaveEnabled = false;
            var name = Regex.Replace(_manga.Name, "[^0-9a-zA-Z]+", " ").Trim();
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Saves", _manga.Site,
                name, _manga.Chapter.ToString(CultureInfo.InvariantCulture));

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            SaveProgress = "Visible";
            for (var i = 0; i < _images.Count; i++)
            {
                if (_isClosing)
                {
                    break;
                }
                ProgressValue = i + 1;
                await SaveImage(_images[i].ToString(), i + 1, folder);
                await Task.Delay(50);
            }
            if (File.Exists($"{folder}.zip"))
            {
                File.Delete($"{folder}.zip");
            }
            ZipFile.CreateFromDirectory(folder, $"{folder}.zip", CompressionLevel.Optimal, true);
            Directory.Delete(folder, true);
            SaveProgress = "Collapsed";
            SaveEnabled = true;
        }

        private async Task SaveImage(string url, int num, string folder)
        {
            try
            {
                var lastSlash = url.LastIndexOf('/');
                var guid = url.Substring(lastSlash + 1);
                var client = new WebClient();
                var extension = guid.Substring(guid.LastIndexOf(".", StringComparison.Ordinal) + 1);
                await client.DownloadFileTaskAsync(url, Path.Combine(folder, $"{num}.{extension}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}