using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Providers;

namespace MangaCheckerV3.Common {
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
        public int Timer;

        public async void Run() {
            Timer = 5;
            while (!Stop)
                if (Timer > 0) {
                    if (!Pause) Timer--;
                    await Task.Delay(1000);
                    Debug.WriteLine(Timer);
                }
                else {
                    Providers.ForEach(async p => {
                        await p.CheckAll();
                        await Task.Delay(1000);
                    });
                    Timer = Database.GetRefreshTime();
                    await Task.Delay(1000);
                    Debug.WriteLine($"Resetting Timer => {Timer}");
                }
        }
    }
}