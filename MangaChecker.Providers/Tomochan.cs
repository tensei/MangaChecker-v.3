using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Tomochan : ISite {
        private readonly WebParser _webParser = new WebParser();
        //http://read.tomochan.today/rss
        public async Task CheckAll() {
            var all = LiteDb.GetMangasFrom(DbName);
            var rss = await _webParser.GetRssFeedAsync("http://read.tomochan.today/rss");
            if (rss == null) return;
            rss.Reverse();
            var openlink = LiteDb.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                var isNew = NewChapterHelper.IsNew(manga, rssItemObject.Category, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
                await Task.Delay(100);
            }
        }

        public async Task<object> CheckOne(object manga) {
            throw new NotImplementedException();
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbName => "Tomochan";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://read.tomochan.today/";
    }
}