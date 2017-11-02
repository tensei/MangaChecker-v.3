using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;

namespace MangaChecker.Providers
{
    public class ProviderService : IProviderService, INotifyPropertyChanged
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;

        private int _active;
        private int _currentProviderIndex;

        public ProviderService(IProviderSet providerSet, IDbContext dbContext, Logger logger)
        {
            Providers = providerSet.GetAll;
            _dbContext = dbContext;
            _logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Close()
        {
            Stop = true;
        }

        public List<IProvider> Providers { get; }
        public bool Pause { get; set; } = false;
        public bool Stop { get; set; }
        public int Timer { get; set; }
        public string Status { get; set; }

        public bool Add(IProvider site)
        {
            if (Providers.ToList().Contains(site))
            {
                return false;
            }
            Providers.Add(site);
            return true;
        }

        public bool Remove(IProvider site)
        {
            if (!Providers.Contains(site))
            {
                return false;
            }
            Providers.Remove(site);
            return true;
        }

        public async Task Run()
        {
            Timer = 5;
            while (!Stop)
            {
                if (Timer > 0)
                {
                    Status = $"Checking in {Timer} seconds.";
                    if (!Pause)
                    {
                        Timer--;
                    }
                    await Task.Delay(1000);
                }
                else
                {
                    var settings = _dbContext.GetAllSettings();
                    var an = settings.Where(s => s.Active == 1).Select(s => s.Setting).ToList();
                    _active = settings.Count(s => s.Active == 1);
                    var providers = Providers.Where(p => an.Contains(p.DbName)).ToList();
                    for (var i = 0; i < providers.Count; i++)
                    {
                        var setting = settings.First(s => s.Setting == providers[i].DbName);
                        if (setting.Active == 0)
                        {
                            continue;
                        }
                        _currentProviderIndex = i;
                        Status = $"[{i}/{_active}]Checking {providers[i].DbName}...";
                        try
                        {
                            await providers[i].CheckAll(ChangeStatus);
                        }
                        catch (Exception e)
                        {
                            _logger.Log.Error(e);
                        }
                        await Task.Delay(1000);
                    }
                    Timer = _dbContext.GetRefreshTime();
                    await Task.Delay(1000);
                }
            }
        }

        private void ChangeStatus(IManga manga)
        {
            Status = $"{DateTime.Now:t} [{_currentProviderIndex}/{_active}] [{manga.Site}] Checking {manga.Name} ...";
        }
    }
}