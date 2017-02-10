using System.Threading.Tasks;
using System.Windows.Input;
using MangaChecker.Data.Models;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels.Adding_ViewModels {
    [ImplementPropertyChanged]
    public class NormalViewModel {
        public NormalViewModel() {
            GetInfoCommand = new ActionCommand(async () => await GetInfo());
            AddMangaCommand = new ActionCommand(Add);
        }

        public string Link { get; set; }

        public bool AddIsEnabled { get; set; }

        public ICommand AddMangaCommand { get; }

        public ICommand GetInfoCommand { get; }

        public Manga Manga { get; set; }

        public bool Loading { get; set; }

        public bool GetIsEnabled { get; set; } = true;

        public bool InfoVisibility { get; set; }

        private async Task GetInfo() {
            InfoVisibility = false;
            Loading = true;
            GetIsEnabled = false;
            await Task.Delay(5000);
            AddIsEnabled = true;
            Loading = false;
#if DEBUG
            InfoVisibility = true;
            Manga = new Manga {
                Name = "Uratarou",
                Chapter = 22,
                Site = "Jaiminisbox",
                Link = "https://jaiminisbox.com/reader/read/uratarou/en/0/22/",
                Thumbnail =
                    "https://jaiminisbox.com/reader/content/comics/uratarou_57eaf67c12054/uratarou-thumbnail.jpg",
                Rss = "https://jaiminisbox.com/reader/feeds/rss"
            };
#endif
        }

        private void Add() {
            GetIsEnabled = true;
            AddIsEnabled = false;
        }
    }
}