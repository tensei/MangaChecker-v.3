using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using MangaChecker.Database.Enums;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;

namespace MangaChecker.Database {
    public static class LiteDb {
        private const string DatabaseVersion = "1.0.0.5";
        private static readonly string DatabasePath = Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db");

        private static readonly LiteDatabase Db = new LiteDatabase(DatabasePath);

        private static readonly Dictionary<string, string> DefaultVersions = new Dictionary<string, string> {
            {"db", "1.0.0.0"}
        };

        static LiteDb() {
        }

        public static void Dispose() {
            Db.Dispose();
        }

        public static event EventHandler<MangaEnum> MangaEvent;
        public static event EventHandler<DatabaseEnum> DbEvent;
        public static event EventHandler<SettingEnum> SettingEvent;

        public static IOrderedEnumerable<Manga> GetHistory() {
            var query = Db.GetCollection<Manga>("History").FindAll();
            var ordered = query.OrderByDescending(m => m.Added);
            MangaEvent?.Invoke(ordered, MangaEnum.GetHistory);
            return ordered;
        }

        public static IEnumerable<Manga> GetAllMangas() {
            var query = Db.GetCollection<Manga>("Manga").FindAll();
            MangaEvent?.Invoke(query, MangaEnum.Get);
            return query;
        }

        public static IOrderedEnumerable<Manga> GetAllNewMangas() {
            var query = Db.GetCollection<Manga>("NewManga").FindAll();
            var ordered = query.OrderBy(m => m.Added);
            MangaEvent?.Invoke(ordered, MangaEnum.New);
            return ordered;
        }

        public static IEnumerable<Manga> GetMangasFrom(string site) {
            var query = Db.GetCollection<Manga>("Manga").Find(s => s.Site == site);
            MangaEvent?.Invoke(query, MangaEnum.Get);
            return query;
        }

        public static void InsertManga(Manga manga) {
            var query = Db.GetCollection<Manga>("Manga");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.Insert);
        }

        public static void InsertHistory(Manga manga) {
            var query = Db.GetCollection<Manga>("History");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.InsertHistory);
        }

        public static void InsertNewManga(Manga manga) {
            var query = Db.GetCollection<Manga>("NewManga");
            query.Insert(manga);
            MangaEvent?.Invoke(manga, MangaEnum.InsertNewManga);
        }

        public static void DeleteNewManga(Manga manga) {
            var query = Db.GetCollection<Manga>("NewManga");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.DeleteNewManga);
        }

        public static void Update(Manga manga, bool history = false) {
            var query = Db.GetCollection<Manga>("Manga");
            query.Update(manga);
            if (history) return;
            MangaEvent?.Invoke(manga, MangaEnum.Update);
        }

        public static void UpdateMangaTrans(List<Manga> manga, bool history = false) {
            using (var _db = new LiteDatabase(DatabasePath)) {
                var query = _db.GetCollection<Manga>("Manga");
                using (var trans1 = _db.BeginTrans()) {
                    query.Update(manga);
                    trans1.Commit();
                }
            }
            if (history) return;
            MangaEvent?.Invoke(manga, MangaEnum.Update);
        }

        public static void Delete(Manga manga) {
            var query = Db.GetCollection<Manga>("Manga");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.Delete);
        }

        public static void DeleteHistory(Manga manga) {
            var query = Db.GetCollection<Manga>("History");
            query.Delete(manga.MangaId);
            MangaEvent?.Invoke(manga, MangaEnum.DeleteHistory);
        }

        public static List<Settings> GetAllSettings() {
            var query = Db.GetCollection<Settings>("Settings").FindAll().ToList();
            SettingEvent?.Invoke(query, SettingEnum.Get);
            return query;
        }

        public static Settings GetSettingsFor(string setting) {
            var query = Db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == setting);
            SettingEvent?.Invoke(query, SettingEnum.Get);
            return query;
        }

        public static void SaveSettings(List<Settings> settings) {
            var query = Db.GetCollection<Settings>("Settings");
            using (var trans1 = Db.BeginTrans()) {
                query.Update(settings);
                trans1.Commit();
            }
            SettingEvent?.Invoke(settings, SettingEnum.Update);
        }

        public static int GetRefreshTime() {
            var query = Db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "Refresh Time");
            SettingEvent?.Invoke(query, SettingEnum.Refresh);
            return query.Active;
        }

        public static bool GetOpenLinks() {
            var query = Db.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "Open Links");
            SettingEvent?.Invoke(query, SettingEnum.Refresh);
            return query.OpenLinks;
        }

        private static void UpdateDatabase(Versions dbv, List<IProvider> providers) {
            var set = Db.GetCollection<Settings>("Settings");
            Db.GetCollection<Manga>("History");
            Db.GetCollection<Manga>("NewManga");
            Db.GetCollection<Manga>("Manga");
            var ver = Db.GetCollection<Versions>("Versions");

            var setting = set.FindAll().Select(s => s.Setting).ToArray();
            var versions = ver.FindAll().Select(v => v.Name).ToArray();

            foreach (var defaultSetting in providers)
                if (!setting.Contains(defaultSetting.DbName))
                    set.Insert(new Settings {
                        Setting = defaultSetting.DbName,
                        Link = defaultSetting.LinktoSite,
                        Active = 0,
                        Created = DateTime.Now
                        //OpenLinks = true
                    });
            if (!setting.Contains("Refresh Time"))
                set.Insert(new Settings {
                    Setting = "Refresh Time",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            if (!setting.Contains("Open Links"))
                set.Insert(new Settings {
                    Setting = "Open Links",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            if (!setting.Contains("Batoto Rss"))
                set.Insert(new Settings {
                    Setting = "Batoto Rss",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            foreach (var defaultver in DefaultVersions)
                if (!versions.Contains(defaultver.Key))
                    ver.Insert(new Versions {
                        Name = defaultver.Key,
                        Version = defaultver.Key
                    });
            ver.Update(dbv);
            DbEvent?.Invoke(dbv, DatabaseEnum.Update);
        }

        public static void CreateDatabase(List<IProvider> providers) {
            var set = Db.GetCollection<Settings>("Settings");
            Db.GetCollection<Manga>("Manga");
            Db.GetCollection<Manga>("History");
            Db.GetCollection<Manga>("NewManga");
            var ver = Db.GetCollection<Versions>("Versions");

            ver.Insert(new Versions {
                Name = "db",
                Version = DatabaseVersion
            });

            foreach (var sites in providers)
                set.Insert(new Settings {
                    Setting = sites.DbName,
                    Link = sites.LinktoSite,
                    Active = 0,
                    Created = DateTime.Now
                });
            if (set.FindOne(s => s.Setting == "Refresh Time") == null)
                set.Insert(new Settings {
                    Setting = "Refresh Time",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            if (set.FindOne(s => s.Setting == "Open Links") == null)
                set.Insert(new Settings {
                    Setting = "Open Links",
                    Link = "/",
                    Active = 300,
                    Created = DateTime.Now
                });
            if (set.FindOne(s => s.Setting == "Batoto Rss") == null)
                set.Insert(new Settings {
                    Setting = "Batoto Rss",
                    Link = "/",
                    Active = 0,
                    Created = DateTime.Now
                });
            DbEvent?.Invoke(null, DatabaseEnum.Create);
        }

        public static string CheckDbVersion(List<IProvider> providers) {
            var dbv = Db.GetCollection<Versions>("Versions").FindOne(v => v.Name == "db");
            if (dbv.Version == DatabaseVersion) return null;
            dbv.Version = DatabaseVersion;
            UpdateDatabase(dbv, providers);
            return $"Updated Database to {DatabaseVersion}";
        }
    }
}

//http://stackoverflow.com/questions/1249517/super-simple-example-of-c-sharp-observer-observable-with-delegates

//class Observable {
//    public event EventHandler SomethingHappened;
//    public static event EventHandler SomethingHappened; //switch to this maybe?

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
//    static void Main() {
//        Observable observable = new Observable();
//        Observer observer = new Observer();
//        observable.SomethingHappened += observer.HandleEvent;

//        observable.DoSomething();
//    }
//}