using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;
using SQLite;

namespace MangaChecker.Database {
	public class Database {
		private readonly string _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "mcv3.sqlite");

		private readonly string _databaseVersion = "1.0.0.6";

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
		private readonly Dictionary<string, string> _defaultThemes = new Dictionary<string, string> {
			{"Primary", "blue"},
			{"Accents", "blue"},
			{"Theme", "Dark" }
		};

		public async Task<List<Manga>> GetAllMangas() {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = conn.Table<Manga>();
            var q = await query.ToListAsync();
            return new List<Manga>(q.OrderByDescending(m => m.Updated));
        }
		public async Task InsertManga(Manga manga) {
			var conn = new SQLiteAsyncConnection(_databasePath);
			await conn.InsertAsync(manga);
		}
		public async Task<IOrderedEnumerable<Manga>> GetMangasFrom(string site) {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = conn.Table<Manga>().Where(m => m.Site.ToLower().Equals(site.ToLower()));
            var q = await query.ToListAsync();
		    return q.OrderByDescending(m => m.Updated);
		}
		public async Task Update(Manga manga) {
			var conn = new SQLiteAsyncConnection(_databasePath);
			await conn.UpdateAsync(manga);
		}
		public async Task Delete(Manga manga) {
			var conn = new SQLiteAsyncConnection(_databasePath);
			await conn.DeleteAsync(manga);
		}
		public async Task<List<Settings>> GetAllSettings() {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = conn.Table<Settings>();
			return await query.ToListAsync();
		}
		public async Task<Settings> GetSettingsFor(string setting) {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = await conn.Table<Settings>().Where(s => s.Setting.Equals(setting)).FirstAsync();
			return query;
		}
		public async Task<int> GetRefreshTime() {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = await conn.Table<Settings>().Where(s => s.Setting.Equals("RefreshTime")).ToListAsync();
			return query.First().Active;
		}
		public async Task<List<Theme>> GetThemes() {
			var conn = new SQLiteAsyncConnection(_databasePath);
			var query = await conn.Table<Theme>().ToListAsync();
			return query;
		}
		public void UpdateTheme(string name, string color) {
			var conn = new SQLiteConnection(_databasePath);
			conn.Update(new Theme {
				Name = name,
				Color = color
			});
		}

		private void UpdateDatabase(Versions dbv) {
			var conn = new SQLiteConnection(_databasePath);
			conn.CreateTable<Settings>();
			conn.CreateTable<Manga>();
			conn.CreateTable<Versions>();
			conn.CreateTable<Theme>();

			var setting = conn.Table<Settings>().ToList().Select(s=>s.Setting).ToList();
			var theme = conn.Table<Theme>().ToList().Select(t => t.Name).ToList();
			var versions = conn.Table<Versions>().ToList().Select(v => v.Name).ToList();

			foreach (var defaultSetting in DefaultDatabaseSettings) {
				if (!setting.Contains(defaultSetting.Key)) {
					conn.Insert(new Settings {
						Setting = defaultSetting.Key,
						Link = defaultSetting.Value,
						Active = 0,
						Created = DateTime.Now,
						//OpenLinks = true
					});
				}
			}
			foreach (var defaultTheme in _defaultThemes) {
				if (!theme.Contains(defaultTheme.Key)) {
					conn.Insert(new Theme {
						Name = defaultTheme.Key,
						Color = defaultTheme.Value
					});
				}
			}
			foreach (var defaultver in _defaultVersions) {
				if (!versions.Contains(defaultver.Key)) {
					conn.Insert(new Versions {
						Name = defaultver.Key,
						Version = defaultver.Key
					});
				}
			}
			conn.Update(dbv);
		}

		public void CreateDatabase() {
			var conn = new SQLiteConnection(_databasePath);
			conn.CreateTable<Settings>();
			conn.CreateTable<Manga>();
			conn.CreateTable<Versions>();
			conn.CreateTable<Theme>();

			conn.Insert(new Versions {
				Name = "db",
				Version = _databaseVersion
			});

			foreach (var sites in DefaultDatabaseSettings) {
				conn.Insert(new Settings {
					Setting = sites.Key,
					Link = sites.Value,
					Active = 0,
					Created = DateTime.Now
				});
			}
			conn.Insert(new Settings {
				Setting = "RefreshTime",
				Link = "/",
				Active = 300,
				Created = DateTime.Now
			});
			conn.Insert(new Theme {
				Name = "Primary",
				Color = "blue"
			});
			conn.Insert(new Theme {
				Name = "Accents",
				Color = "blue"
			});
			conn.Insert(new Theme {
				Name = "Theme",
				Color = "Dark"
			});


		}

		public string CheckDbVersion() {
			var conn = new SQLiteConnection(_databasePath);
			var dbv = conn.Table<Versions>().FirstOrDefault(v => v.Name.Equals("db"));
			if (dbv.Version == _databaseVersion) return null;
			dbv.Version = _databaseVersion;
			UpdateDatabase(dbv);
			return $"Updated Database to {_databaseVersion}";
		}
	}
}