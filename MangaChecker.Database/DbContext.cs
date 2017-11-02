using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;

namespace MangaChecker.Database
{
    public class DbContext : IDbContext
    {
        private const string DatabaseVersion = "1.0.0.7";
        private readonly string _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db");

        private readonly LiteDatabase _db;

        private readonly Dictionary<string, string> _defaultVersions = new Dictionary<string, string>
        {
            {"db", "1.0.0.0"}
        };

        public DbContext()
        {
            _db = new LiteDatabase(_databasePath);
        }

        public event EventHandler<MangaEnum> MangaEvent;
        public event EventHandler<DatabaseEnum> DbEvent;
        public event EventHandler<SettingEnum> SettingEvent;

        public IOrderedEnumerable<Manga> GetHistory()
        {
            var query = _db.GetCollection<Manga>("History").FindAll();
            var ordered = query.OrderByDescending(m => m.Added);
            MangaEvent?.Invoke(ordered, MangaEnum.GetHistory);
            return ordered;
        }

        public IEnumerable<Manga> GetAllMangas()
        {
            var query = _db.GetCollection<Manga>("Manga").FindAll();
            MangaEvent?.Invoke(query, MangaEnum.Get);
            return query;
        }

        public IOrderedEnumerable<Manga> GetAllNewMangas()
        {
            var query = _db.GetCollection<Manga>("NewManga").FindAll();
            var ordered = query.OrderBy(m => m.Added);
            MangaEvent?.Invoke(ordered, MangaEnum.New);
            return ordered;
        }

        public IEnumerable<Manga> GetMangasFrom(string site)
        {
            var query = _db.GetCollection<Manga>("Manga").Find(s => s.Site == site);
            MangaEvent?.Invoke(query, MangaEnum.Get);
            return query;
        }

        public void InsertManga(Manga manga)
        {
            var query = _db.GetCollection<Manga>("Manga");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.Insert);
        }

        public void InsertHistory(Manga manga)
        {
            var query = _db.GetCollection<Manga>("History");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.InsertHistory);
        }

        public void InsertNewManga(Manga manga)
        {
            var query = _db.GetCollection<Manga>("NewManga");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.InsertNewManga);
        }

        public void DeleteNewManga(Manga manga)
        {
            var query = _db.GetCollection<Manga>("NewManga");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.DeleteNewManga);
        }

        public void Update(Manga manga, bool history = false)
        {
            var query = _db.GetCollection<Manga>("Manga");
            query.Update(manga);
            if (history)
            {
                return;
            }
            MangaEvent?.Invoke(manga, MangaEnum.Update);
        }

        public void UpdateMangaTrans(List<Manga> manga, bool history = false)
        {
            var query = _db.GetCollection<Manga>("Manga");
            query.Update(manga);
            if (history)
            {
                return;
            }
            MangaEvent?.Invoke(manga, MangaEnum.Update);
        }

        public void Delete(Manga manga)
        {
            var query = _db.GetCollection<Manga>("Manga");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.Delete);
        }

        public void DeleteHistory(Manga manga)
        {
            var query = _db.GetCollection<Manga>("History");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.DeleteHistory);
        }

        public List<Settings> GetAllSettings()
        {
            var query = _db.GetCollection<Settings>("Settings").FindAll().ToList();
            SettingEvent?.Invoke(query, SettingEnum.Get);
            return query;
        }

        public Settings GetSettingsFor(string setting)
        {
            var query = _db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == setting);
            SettingEvent?.Invoke(query, SettingEnum.Get);
            return query;
        }

        public void SaveSettings(List<Settings> settings)
        {
            var query = _db.GetCollection<Settings>("Settings");
            query.Update(settings);
            SettingEvent?.Invoke(settings, SettingEnum.Update);
        }

        public int GetRefreshTime()
        {
            var query = _db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "Refresh Time");
            SettingEvent?.Invoke(query, SettingEnum.Refresh);
            return query.Active;
        }

        public bool GetOpenLinks()
        {
            var query = _db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "Open Links");
            SettingEvent?.Invoke(query, SettingEnum.Refresh);
            return query.OpenLinks;
        }

        public void UpdateDatabase(Versions dbv, Dictionary<string, string> providers)
        {
            var set = _db.GetCollection<Settings>("Settings");
            _db.GetCollection<Manga>("History");
            _db.GetCollection<Manga>("NewManga");
            _db.GetCollection<Manga>("Manga");
            var ver = _db.GetCollection<Versions>("Versions");

            var setting = set.FindAll().Select(s => s.Setting).ToArray();
            var versions = ver.FindAll().Select(v => v.Name).ToArray();

            foreach (var defaultSetting in providers)
            {
                if (!setting.Contains(defaultSetting.Key))
                {
                    set.Insert(new Settings
                    {
                        Setting = defaultSetting.Key,
                        Link = defaultSetting.Value,
                        Active = 0,
                        Created = DateTime.Now
                        //OpenLinks = true
                    });
                }
            }
            if (!setting.Contains("Refresh Time"))
            {
                set.Insert(new Settings
                {
                    Setting = "Refresh Time",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            }
            if (!setting.Contains("Open Links"))
            {
                set.Insert(new Settings
                {
                    Setting = "Open Links",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            }
            if (!setting.Contains("Batoto Rss"))
            {
                set.Insert(new Settings
                {
                    Setting = "Batoto Rss",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            }
            foreach (var defaultver in _defaultVersions)
            {
                if (!versions.Contains(defaultver.Key))
                {
                    ver.Insert(new Versions
                    {
                        Name = defaultver.Key,
                        Version = defaultver.Key
                    });
                }
            }
            ver.Update(dbv);
            DbEvent?.Invoke(dbv, DatabaseEnum.Update);
        }

        public void CreateDatabase(Dictionary<string, string> providers)
        {
            var set = _db.GetCollection<Settings>("Settings");
            _db.GetCollection<Manga>("Manga");
            _db.GetCollection<Manga>("History");
            _db.GetCollection<Manga>("NewManga");
            var ver = _db.GetCollection<Versions>("Versions");

            ver.Insert(new Versions
            {
                Name = "db",
                Version = DatabaseVersion
            });

            foreach (var sites in providers)
            {
                set.Insert(new Settings
                {
                    Setting = sites.Key,
                    Link = sites.Value,
                    Active = 0,
                    Created = DateTime.Now
                });
            }
            if (set.FindOne(s => s.Setting == "Refresh Time") == null)
            {
                set.Insert(new Settings
                {
                    Setting = "Refresh Time",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            }
            if (set.FindOne(s => s.Setting == "Open Links") == null)
            {
                set.Insert(new Settings
                {
                    Setting = "Open Links",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            }
            if (set.FindOne(s => s.Setting == "Batoto Rss") == null)
            {
                set.Insert(new Settings
                {
                    Setting = "Batoto Rss",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            }
            DbEvent?.Invoke(null, DatabaseEnum.Create);
        }

        public string CheckDbVersion(Dictionary<string, string> providers)
        {
            var dbv = _db.GetCollection<Versions>("Versions").FindOne(v => v.Name == "db");
            if (dbv.Version == DatabaseVersion)
            {
                return null;
            }
            dbv.Version = DatabaseVersion;
            UpdateDatabase(dbv, providers);
            return $"Updated Database to {DatabaseVersion}";
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

//http://stackoverflow.com/questions/1249517/super-simple-example-of-c-sharp-observer-observable-with-delegates

//class Observable {
//    public event EventHandler SomethingHappened;
//    public event EventHandler SomethingHappened; //switch to this maybe?

//    public void DoSomething() {
//        EventHandler handler = SomethingHappened;
//        if (handler != null) {
//            handler(this, EventArgs.Empty);
//        }
//    }
//}

//class Observer {
//    public void HandleEvent(object sender, EventArgs args) {
//        Console.WriteLine("Something happened to " + sender);
//    }
//}

//class Test {
//    void Main() {
//        Observable observable = new Observable();
//        Observer observer = new Observer();
//        observable.SomethingHappened += observer.HandleEvent;

//        observable.DoSomething();
//    }
//}