using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MangaChecker.Database.Tables;

namespace MangaChecker.Database {
	public class Database {
		private readonly string _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "mcv3.db");

		private readonly string _databaseVersion = "1.0.0.8";

		private static readonly Dictionary<string, string> DefaultDatabaseSettings = new Dictionary<string, string> {
			{"Mangafox", "http://mangafox.me/"},
			{"Mangahere", "http://mangahere.co/"},
			{"Mangareader", "http://www.mangareader.net/"},
			{"Mangastream", "http://mangastream.com/"},
			{"Batoto", "http://bato.to/"},
			{"Webtoons", "http://www.webtoons.com/"},
			{"YoManga", "http://yomanga.co/"},
			{"Kissmanga", "http://kissmanga.com/"},
			{"Backlog", "/"},
			{"GoScanlation", "https://gameofscanlation.moe/"},
			{"KireiCake", "http://kireicake.com/" },
			{"Jaiminisbox", "https://jaiminisbox.com/" },
			{"HeyManga", "https://www.heymanga.me/" }
		};

		private readonly Dictionary<string, string> _defaultVersions = new Dictionary<string, string> {
			{"db", "1.0.0.0"}
		};
        public IOrderedEnumerable<Manga> GetAllMangas() {
            IEnumerable<Manga> query;
            using (var conn = new LiteDatabase(_databasePath)) {
                query = conn.GetCollection<Manga>("Manga").FindAll();
                //var qquery = conn.GetCollection("Manga").FindAll();
            }
            return query.OrderByDescending(m => m.Updated);
        }
        public IOrderedEnumerable<Manga> GetMangasFrom(string site) {
            IEnumerable<Manga> query;
            using (var conn = new LiteDatabase(_databasePath)) {
                query = conn.GetCollection<Manga>("Manga").Find(s => s.Site == site);
            }
            return query.OrderByDescending(m => m.Updated);
        }
        public void InsertManga(Manga manga) {
            using (var conn = new LiteDatabase(_databasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Insert(manga);
            }
        }
        public void Update(Manga manga) {
            using (var conn = new LiteDatabase(_databasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Update(manga);
            }
        }
        public void Delete(Manga manga) {
            using (var conn = new LiteDatabase(_databasePath)) {
                var query = conn.GetCollection<Manga>("Manga");
                query.Delete(Query.EQ("_id", manga.MangaId));
            }
        }
        public IEnumerable<Settings> GetAllSettings() {
            IEnumerable<Settings> query;
            using (var conn = new LiteDatabase(_databasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindAll();
            }
            return query;
        }
        public Settings GetSettingsFor(string setting) {
            Settings query;
            using (var conn = new LiteDatabase(_databasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindOne(s=> s.Setting == setting);
            }
            return query;
        }
        public int GetRefreshTime() {
            Settings query;
            using (var conn = new LiteDatabase(_databasePath)) {
                query = conn.GetCollection<Settings>("Settings").FindOne(s => s.Setting == "RefreshTime");
            }
            return query.Active;
        }

		private void UpdateDatabase(Versions dbv) {
            using (var conn = new LiteDatabase(_databasePath)) {
                var set = conn.GetCollection<Settings>("Settings");
                conn.GetCollection<Manga>("Manga");
                var ver = conn.GetCollection<Versions>("Versions");

                var setting = set.Find(Query.All(Query.Descending)).Select(s => s.Setting);
                var versions = ver.Find(Query.All(Query.Descending)).Select(v => v.Name);

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
                foreach (var defaultver in _defaultVersions) {
                    if (!versions.Contains(defaultver.Key)) {
                        ver.Insert(new Versions {
                            Name = defaultver.Key,
                            Version = defaultver.Key
                        });
                    }
                }
                ver.Update(dbv);
            }
        }

		public void CreateDatabase() {
		    using (var conn = new LiteDatabase(_databasePath)) {
		        var set = conn.GetCollection<Settings>("Settings");
		        conn.GetCollection<Manga>("Manga");
		        var ver = conn.GetCollection<Versions>("Versions");

		        ver.Insert(new Versions {
		            Name = "db",
		            Version = _databaseVersion
		        });

		        foreach (var sites in DefaultDatabaseSettings) {
		            set.Insert(new Settings {
		                Setting = sites.Key,
		                Link = sites.Value,
		                Active = 0,
		                Created = DateTime.Now
		            });
		        }
		        if (set.FindOne(Query.EQ("Setting", "RefreshTime")) == null) {
		            set.Insert(new Settings {
		                Setting = "RefreshTime",
		                Link = "/",
		                Active = 300,
		                Created = DateTime.Now
		            });
		        }
		    }
		}

		public string CheckDbVersion() {
		    using (var conn = new LiteDatabase(_databasePath)) {
		        var dbv = conn.GetCollection<Versions>("Versions").FindOne(Query.EQ("Name", "db"));
		        if (dbv.Version == _databaseVersion) return null;
		        dbv.Version = _databaseVersion;
		        UpdateDatabase(dbv);
		        return $"Updated Database to {_databaseVersion}";
		    }
		}
	}
}