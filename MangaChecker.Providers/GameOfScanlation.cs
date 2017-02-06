using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class GameOfScanlation : ISite {
        private readonly WebParser _webParser = new WebParser();

        public async Task CheckAll() {
            var all = LiteDb.GetMangasFrom(DbName);
            var openlink = LiteDb.GetOpenLinks();
            foreach (var manga in all) {
                var rss = await _webParser.GetRssFeedAsync(manga.Rss);
                if (rss == null) continue;
                rss.Reverse();
                foreach (var rssItemObject in rss) {
                    var nc =
                        rssItemObject.Title.ToLower().Replace($"{manga.Name.ToLower()} chapter", string.Empty).Trim();
                    if (nc.Contains(" ")) nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                    var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                }
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

        public string DbName => "GameOfScanlation";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "https://gameofscanlation.moe/";
    }
}