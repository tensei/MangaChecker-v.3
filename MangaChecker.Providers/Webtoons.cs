using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Database;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Webtoons : IProvider {
        private readonly IWebParser _webParser;
        private readonly ILiteDb _liteDb;
        private readonly INewChapterHelper _newChapterHelper;

        public Webtoons(IWebParser webParser, ILiteDb liteDb, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _liteDb = liteDb;
            _newChapterHelper = newChapterHelper;
        }
        public async Task CheckAll() {
            var all = _liteDb.GetMangasFrom(DbName);
            var openlink = _liteDb.GetOpenLinks();
            foreach (var manga in all) {
                var rss = await _webParser.GetRssFeedAsync(manga.Rss);
                if (rss == null) {
                    continue;
                }
                rss.Reverse();
                foreach (var rssItemObject in rss) {
                    var title = rssItemObject.Title;
                    var nc =
                        Regex.Match(title, @"(?<other>ep\. |episode )(?<chapter>\d+.?\d+)", RegexOptions.IgnoreCase);
                    var ch = nc.Groups["chapter"].Value;
                    if (string.IsNullOrWhiteSpace(ch)) {
                        ch = rssItemObject.Title;
                    }
                    var isNew = _newChapterHelper.IsNew(manga, ch.Trim('.').Trim(), rssItemObject.PubDate,
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

        public string DbName => "Webtoons";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://www.webtoons.com/";
    }
}