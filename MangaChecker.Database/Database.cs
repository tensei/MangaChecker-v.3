using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MangaChecker.Database.Tables;

namespace MangaChecker.Database {
    public static class Database {
        private static readonly string DatabasePath = Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db");

        private const string DatabaseVersion = "1.0.0.1";
        public static List<IDatabaseObserver> Observers = new List<IDatabaseObserver>();

        //public static event EventHandler SomethingHappened; //switch to this maybe?

        private static readonly Dictionary<string, string> DefaultDatabaseSettings = new Dictionary<string, string> {
            {"Mangafox", "http://mangafox.me/"},
            {"Mangahere", "http://mangahere.co/"},
            {"Mangareader", "http://www.mangareader.net/"},
            {"Mangastream", "http://mangastream.com/"},
            {"Batoto", "http://bato.to/"},
            {"Webtoons", "http://www.webtoons.com/"},
            {"YoManga", "http://yomanga.co/"},
            {"Kissmanga", "http://kissmanga.com/"},
            {"GameOfScanlation", "https://gameofscanlation.moe/"},
            {"KireiCake", "http://kireicake.com/" },
            {"Jaiminisbox", "https://jaiminisbox.com/" },
            {"HeyManga", "https://www.heymanga.me/" }
        };

        private static readonly Dictionary<string, string> DefaultVersions = new Dictionary<string, string> {
            {"db", "1.0.0.0"}
        };
        public static IOrderedEnumerable<Manga> GetAllMangas() {
            IEnumerable<Manga> query;
            using (var conn = new LiteDatabase(DatabasePath)) {
                query = conn.GetCollection<Manga>("Manga").FindAll();
            }
            var ordered = query.OrderByDescending(m => m.Updated);
            Observers?.ForEach(o => o.GetMangaEvent(ordered.ToList(), DatabaseEvent.GET));
            return ordered;
        }
        public static IOrderedEnumerable<Manga> GetMangasFrom(string site) {
            IEnumerable<Manga> query;
            using (var conn = new LiteDatabase(DatabasePath)) {
                query = conn.GetCollection<Manga>("Manga").Find(s => s.Site == site);
            }
            var ordered = query.OrderByDescending(m => m.Updated);
            Observers?.ForEach(o => o.GetMangaEvent(ordered.ToList(), DatabaseEvent.GET));
            return ordered;
        }
        public static void InsertManga(Manga manga) {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Insert(manga);
            }
            Observers?.ForEach(o => o.MangaEvent(manga, DatabaseEvent.INSERT));
        }
        public static void Update(Manga manga) {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Update(manga);
            }
            Observers?.ForEach(o => o.MangaEvent(manga, DatabaseEvent.UPDATE));
        }
        public static void Delete(Manga manga) {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Delete(manga.MangaId);
            }
            Observers?.ForEach(o => o.MangaEvent(manga, DatabaseEvent.DELETE));
        }
        public static List<Settings> GetAllSettings() {
            List<Settings> query;
            using (var conn = new LiteDatabase(DatabasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindAll().ToList();
            }
            Observers?.ForEach(o => o.GetSettingEvent(query.ToList(), DatabaseEvent.GET));
            return query;
        }
        public static Settings GetSettingsFor(string setting) {
            Settings query;
            using (var conn = new LiteDatabase(DatabasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindOne(s => s.Setting == setting);
            }
            Observers?.ForEach(o => o.SettingEvent(query, DatabaseEvent.GET));
            return query;
        }
        public static void SaveSettings(List<Settings> settings) {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var query = conn.GetCollection<Settings>("Settings");
                using (var trans1 = conn.BeginTrans()) {
                    settings.ForEach(s => query.Update(s));
                    trans1.Commit();
                }
            }
            Observers?.ForEach(o => o.SaveSettingEvent(settings, DatabaseEvent.UPDATE));
        }
        public static int GetRefreshTime() {
            Settings query;
            using (var conn = new LiteDatabase(DatabasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "RefreshTime");
            }
            Observers?.ForEach(o => o.SettingEvent(query, DatabaseEvent.GET));
            return query.Active;
        }

        private static void UpdateDatabase(Versions dbv) {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var set = conn.GetCollection<Settings>("Settings");
                conn.GetCollection<Manga>("Manga");
                var ver = conn.GetCollection<Versions>("Versions");

                var setting = set.FindAll().Select(s => s.Setting).ToArray();
                var versions = ver.FindAll().Select(v => v.Name).ToArray();

                foreach (var defaultSetting in DefaultDatabaseSettings) {
                    if (!setting.Contains(defaultSetting.Key)) {
                        set.Insert(new Settings {
                            Setting = defaultSetting.Key,
                            Link = defaultSetting.Value,
                            Active = 0,
                            Created = DateTime.Now,
                            //OpenLinks = true
                        });
                    }
                }
                if (!setting.Contains("Refresh Time")) {
                    set.Insert(new Settings {
                        Setting = "Refresh Time",
                        Link = "/",
                        Active = 300,
                        Created = DateTime.Now
                    });
                }
                if (!setting.Contains("Open Links")) {
                    set.Insert(new Settings {
                        Setting = "Open Links",
                        Link = "/",
                        Active = 0,
                        Created = DateTime.Now
                    });
                }
                if (!setting.Contains("Batoto Rss")) {
                    set.Insert(new Settings {
                        Setting = "Batoto Rss",
                        Link = "/",
                        Active = 0,
                        Created = DateTime.Now
                    });
                }
                foreach (var defaultver in DefaultVersions) {
                    if (!versions.Contains(defaultver.Key)) {
                        ver.Insert(new Versions {
                            Name = defaultver.Key,
                            Version = defaultver.Key
                        });
                    }
                }
                ver.Update(dbv);
            }
            Observers?.ForEach(o => o.DatabaseEvent(DatabaseEvent.DBUPDATE));
        }

        public static void CreateDatabase() {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var set = conn.GetCollection<Settings>("Settings");
                conn.GetCollection<Manga>("Manga");
                var ver = conn.GetCollection<Versions>("Versions");

                ver.Insert(new Versions {
                    Name = "db",
                    Version = DatabaseVersion
                });

                foreach (var sites in DefaultDatabaseSettings) {
                    set.Insert(new Settings {
                        Setting = sites.Key,
                        Link = sites.Value,
                        Active = 0,
                        Created = DateTime.Now
                    });
                }
                if (set.FindOne(s => s.Setting == "Refresh Time") == null) {
                    set.Insert(new Settings {
                        Setting = "Refresh Time",
                        Link = "/",
                        Active = 300,
                        Created = DateTime.Now
                    });
                }
                if (set.FindOne(s => s.Setting == "Open Links") == null) {
                    set.Insert(new Settings {
                        Setting = "Open Links",
                        Link = "/",
                        Active = 300,
                        Created = DateTime.Now
                    });
                }
                if (set.FindOne(s => s.Setting == "Batoto Rss") == null) {
                    set.Insert(new Settings {
                        Setting = "Batoto Rss",
                        Link = "/",
                        Active = 0,
                        Created = DateTime.Now
                    });
                }
            }
            Observers?.ForEach(o => o.DatabaseEvent(DatabaseEvent.DBCREATE));
        }

        public static string CheckDbVersion() {
            using (var conn = new LiteDatabase(DatabasePath)) {
                var dbv = conn.GetCollection<Versions>("Versions").FindOne(v => v.Name == "db");
                if (dbv.Version == DatabaseVersion) return null;
                dbv.Version = DatabaseVersion;
                UpdateDatabase(dbv);
                return $"Updated Database to {DatabaseVersion}";
            }
        }
    }
}

//http://stackoverflow.com/questions/1249517/super-simple-example-of-c-sharp-observer-observable-with-delegates

//class Observable {
//    public event EventHandler SomethingHappened;

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