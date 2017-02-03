using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Mangastream : ISite {
        public async Task CheckAll() {
            var all = LiteDB.GetMangasFrom(DbSettingName());
            var rss = await WebParser.GetRssFeedAsync("http://mangastream.com/rss");
            if (rss == null) return;
            rss.Reverse();
            var openlink = LiteDB.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) continue;
                var nc = rssItemObject.Title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
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
            return "Mangastream";
        }

        public Regex LinkRegex() {
            return new Regex("");
        }
    }
}