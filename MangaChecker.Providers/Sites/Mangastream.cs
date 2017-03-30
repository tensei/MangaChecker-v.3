using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites {
    public class Mangastream : IProvider {
        private readonly IWebParser _webParser;
        private readonly ILiteDb _liteDb;
        private readonly INewChapterHelper _newChapterHelper;

        public Mangastream(IWebParser webParser, ILiteDb liteDb, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _liteDb = liteDb;
            _newChapterHelper = newChapterHelper;
        }
        public async Task CheckAll() {
            var all = _liteDb.GetMangasFrom(DbName);
            var rss = await _webParser.GetRssFeedAsync("http://mangastream.com/rss");
            if (rss == null) {
                return;
            }
            rss.Reverse();
            var openlink = _liteDb.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) {
                    continue;
                }
                var nc = rssItemObject.Title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            var l = new List<object>();
            var blankurl = url.Remove(url.Length - 1);
            //Last Page (20)
            var document = await _webParser.GetHtmlSourceDocumentAsync(url);
            var pages = Regex.Match(document.DocumentElement.InnerHtml, @"Last Page \((\d+)\)").Groups[1].Value;
            int.TryParse(pages, out int p);
            //l.Add(document.All.First(i=> i.LocalName=="img" && i.GetAttribute("id") == "manga-page").GetAttribute("src"));
            for (var i = 1; i <= p; i++) {
                document = await _webParser.GetHtmlSourceDocumentAsync(blankurl + i);
                var img =
                    document.All.First(im => im.LocalName == "img" && im.GetAttribute("id") == "manga-page")
                        .GetAttribute("src");
                if (img.StartsWith("//")) {
                    img = $"http:{img}";
                }
                l.Add(img);
            }
            return new Tuple<List<object>, int>(l, p);
        }

        public async Task<object> CheckOne(object manga) {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbName => "Mangastream";

        public bool LinkIsMatch(string link) {
            var regex = new Regex("^http://mangastream.com/r/(.+)/[0-9]+/[0-9]+/1$");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => true;
        public string LinktoSite => "http://mangastream.com/";
    }
}