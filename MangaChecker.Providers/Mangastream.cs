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
        private readonly WebParser _webParser = new WebParser();
        public async Task CheckAll() {
            var all = LiteDb.GetMangasFrom(DbName);
            var rss = await _webParser.GetRssFeedAsync("http://mangastream.com/rss");
            if (rss == null) return;
            rss.Reverse();
            var openlink = LiteDb.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) continue;
                var nc = rssItemObject.Title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<object> CheckOne(object manga) {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbName => "Mangastream";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://mangastream.com/";
    }
}