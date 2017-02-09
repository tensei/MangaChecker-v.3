using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interface;
using MangaChecker.Database;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Mangafox : IProvider {
        private readonly IWebParser _webParser;
        private readonly ILiteDb _liteDb;
        private readonly INewChapterHelper _newChapterHelper;

        public Mangafox(IWebParser webParser, ILiteDb liteDb, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _liteDb = liteDb;
            _newChapterHelper = newChapterHelper;
        }
        public async Task CheckAll() {
            var all = _liteDb.GetMangasFrom(DbName);
            var openlink = _liteDb.GetOpenLinks();
            foreach (var manga in all) {
                if (string.IsNullOrEmpty(manga.Rss)) {
                    Logger.Log.Warn($"MANGAFOX {manga.Name} missing rss feed link");
                    continue;
                }
                var rss = await _webParser.GetRssFeedAsync(manga.Rss);
                if (rss == null) {
                    continue;
                }
                rss.Reverse();
                foreach (var rssItemObject in rss) {
                    // sample title => Boku no Hero Academia Vol TBD Ch 125
                    var re = Regex.Match(rssItemObject.Title,
                        @"(?<name>.+)(?<vol>[vol tbd0-9]+)?[ch ]+?(?<chapter>[0-9\.]+)", RegexOptions.IgnoreCase);
                    var nc = string.IsNullOrEmpty(re.Groups["chapter"].Value)
                        ? rssItemObject.Title.Replace(manga.Name.ToLower(), string.Empty)
                        : re.Groups["chapter"].Value.Trim('.').Trim();
                    var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink, _liteDb);
                    await Task.Delay(100);
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

        public string DbName => "Mangafox";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://mangafox.me/";
    }
}