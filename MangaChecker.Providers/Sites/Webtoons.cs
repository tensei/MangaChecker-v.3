using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites {
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
                        Regex.Match(title, @"(?<other>ep\.|episode ) ?(?<chapter>\d+.?\d+|[0-9])", RegexOptions.IgnoreCase);
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

        public bool LinkIsMatch(string link) {
            var regex = new Regex("");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://www.webtoons.com/";
    }
}