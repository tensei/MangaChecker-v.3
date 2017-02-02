using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class YoManga : ISite {
        public async Task CheckAll() {
            var all = LiteDB.GetMangasFrom(DbSettingName());
            var openlink = LiteDB.GetOpenLinks();
            var rss = await WebParser.GetRssFeedAsync("https://yomanga.co/reader/feeds/rss");
            rss.Reverse();
            foreach (var manga in all) {
                foreach (var rssItemObject in rss) {
                    if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) {
                        continue;
                    }
                    var nc = rssItemObject.Title.ToLower().Replace($"{manga.Name.ToLower()} chapter", string.Empty).Trim();
                    if (nc.Contains(" ")) {
                        nc = nc.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                }
            }
        }

        public async Task<IEnumerable<object>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<Manga> CheckOne(Manga manga) {
            throw new NotImplementedException();
        }

        public async Task<Manga> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbSettingName() {
            return "YoManga";
        }

        public Regex LinkRegex() {
            return new Regex("");
        }
    }
}