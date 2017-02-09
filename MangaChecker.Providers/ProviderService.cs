using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaChecker.Utilities;
using PropertyChanged;

namespace MangaChecker.Providers {
    [ImplementPropertyChanged]
    public class ProviderService : IProviderService, IDisposable {

        public ProviderService(IEnumerable<IProvider> provider, ILiteDb liteDb, Logger logger) {
            Providers = provider.ToList();
            _liteDb = liteDb;
            _logger = logger;
        }

        public void Dispose() {
            Stop = true;
        }

        public List<IProvider> Providers { get; }
        private readonly ILiteDb _liteDb;
        private readonly Logger _logger;
        public bool Pause { get; set; } = false;
        public bool Stop { get; set; }
        public int Timer { get; set; }
        public string Status { get; set; }

        public bool Add(IProvider site) {
            if (Providers.ToList().Contains(site)) {
                return false;
            }
            Providers.Add(site);
            return true;
        }

        public bool Remove(IProvider site) {
            if (!Providers.Contains(site)) {
                return false;
            }
            Providers.Remove(site);
            return true;
        }

        public async Task Run() {
            Timer = 5;
            while (!Stop) {
                if (Timer > 0) {
                    Status = $"Checking in {Timer} seconds.";
                    if (!Pause) {
                        Timer--;
                    }
                    await Task.Delay(1000);
                }
                else {
                    foreach (var provider in Providers) {
                        var setting = _liteDb.GetSettingsFor(provider.DbName);
                        if (setting.Active == 0) {
                            continue;
                        }
                        Status = $"Checking {provider.DbName}...";
                        try {
                            await provider.CheckAll();
                        }
                        catch (Exception e) {
                            _logger.Log.Error(e);
                        }
                        await Task.Delay(1000);
                    }
                    Timer = _liteDb.GetRefreshTime();
                    await Task.Delay(1000);
                }
            }
        }
    }
}