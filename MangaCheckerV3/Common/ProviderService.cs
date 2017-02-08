using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Providers;
using MangaChecker.Utilities;
using PropertyChanged;

namespace MangaCheckerV3.Common {
    [ImplementPropertyChanged]
    public class ProviderService : IProviderService , IDisposable{
        public List<IProvider> Providers { get; set; }

        public ProviderService() {
            Providers = new List<IProvider> {
                new Webtoons(),
                new Mangastream(),
                new Tomochan(),
                new YoManga(),
                new Batoto(),
                new GameOfScanlation(),
                new Jaiminisbox(),
                new KireiCake(),
                new Crunchyroll(),
                new HeyManga(),
                new Kissmanga(),
                new Mangafox(),
                new Mangahere(),
                new Mangareader(),
                new Sensescans()
            };
        }
        public bool Pause { get; set; } = false;
        public bool Stop { get; set; }
        public int Timer { get; set; }
        public string Status { get; set; }

        public bool Add(IProvider site) {
            if (Providers.Contains(site)) return false;
            Providers.Add(site);
            return true;
        }

        public bool Remove(IProvider site) {
            if (!Providers.Contains(site)) return false;
            Providers.Remove(site);
            return true;
        }

        public async Task Run() {
            Timer = 5;
            while (!Stop)
                if (Timer > 0) {
                    Status = $"Checking in {Timer} seconds.";
                    if (!Pause) Timer--;
                    await Task.Delay(1000);
                }
                else {
                    foreach (var provider in Providers) {
                        var setting = LiteDb.GetSettingsFor(provider.DbName);
                        if (setting.Active == 0) continue;
                        Status = $"Checking {provider.DbName}...";
                        try {
                            await provider.CheckAll();
                        }
                        catch (Exception e) {
                            Log.Loggger.Error(e);
                        }
                        await Task.Delay(1000);
                    }
                    Timer = LiteDb.GetRefreshTime();
                    await Task.Delay(1000);
                }
        }

        public void Dispose() {
            Stop = true;
        }
    }
}