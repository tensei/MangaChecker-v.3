using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class ProviderService : IProviderService, INotifyPropertyChanged {
        private readonly ILiteDb _liteDb;
        private readonly Logger _logger;
        private int _currentProviderIndex;

        public ProviderService(IProviderSet providerSet, ILiteDb liteDb, Logger logger) {
            Providers = providerSet.GetAll;
            _liteDb = liteDb;
            _logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Close() {
            Stop = true;
        }

        public List<IProvider> Providers { get; }
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
                    for (var i = 0; i < Providers.Count; i++) {
                        var setting = _liteDb.GetSettingsFor(Providers[i].DbName);
                        if (setting.Active == 0) {
                            continue;
                        }
                        _currentProviderIndex = i;
                        Status = $"[{i}/{Providers.Count}]Checking {Providers[i].DbName}...";
                        try {
                            await Providers[i].CheckAll(ChangeStatus);
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

        private void ChangeStatus(IManga manga) {
            Status = $"[{_currentProviderIndex}/{Providers.Count}][{manga.Site}] Checking {manga.Name} ...";
        }
    }
}