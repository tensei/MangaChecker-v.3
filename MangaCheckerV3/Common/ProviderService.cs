using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Providers;
using MangaCheckerV3.ViewModels;
using PropertyChanged;

namespace MangaCheckerV3.Common {
    [ImplementPropertyChanged]
    public class ProviderService {
        public static List<ISite> Providers = new List<ISite> {
            new Webtoons(),
            new YoManga(),
            new Batoto(),
            new GameOfScanlation(),
            new HeyManga(),
            new Jaiminisbox(),
            new KireiCake(),
            new Kissmanga(),
            new Mangafox(),
            new Mangahere(),
            new Mangareader(),
            new Mangastream()
        };

        public static bool Pause = false;
        public static bool Stop = false;
        public int Timer { get; set; }
        public string Status { get; set; }

        public async Task Run() {
            Timer = 5;
            while (!Stop) {
                if (Timer > 0) {
                    Status = $"Checking in {Timer} seconds.";
                    if (!Pause) Timer--;
                    await Task.Delay(1000);
                }
                else {
                    foreach (var provider in Providers) {
                        Status = $"Checking {provider.GetType().Name}...";
                        //await p.CheckAll();
                        await Task.Delay(1000);
                    }
                    Timer = Database.GetRefreshTime();
                    await Task.Delay(1000);
                }
            }
        }
    }
}