using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites {
    public class Kissmanga : IProvider {
        private readonly IWebParser _webParser;
        private readonly ILiteDb _liteDb;
        private readonly INewChapterHelper _newChapterHelper;

        public Kissmanga(IWebParser webParser, ILiteDb liteDb, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _liteDb = liteDb;
            _newChapterHelper = newChapterHelper;
        }
        public async Task CheckAll() {
            var all = _liteDb.GetMangasFrom(DbName);
            var openlink = _liteDb.GetOpenLinks();
            foreach (var manga in all) {
                var html = await _webParser.GetHtmlSourceDucumentAsync(manga.BaseMangaLink);
                if (html == null) {
                    continue;
                }
                var tr = html.All.Where(t => t.LocalName == "tr" && t.Children.Length == 2);
                foreach (var element in tr.Reverse()) {
                    var title = element.Children[0].TextContent.Trim();
                    if (title.Contains("Chapter Name")) {
                        continue;
                    }
                    var newDate = DateTime.Parse(element.Children[1].TextContent.Trim('\n').Trim(),
                        CultureInfo.InvariantCulture);
                    var link = "http://kissmanga.com" + element.Children[0].Children[0].GetAttribute("href");
                    if (!title.ToLower().Contains(manga.Name.ToLower())) {
                        continue;
                    }
                    var nc =
                        title.ToLower().Replace($"{manga.Name.ToLower()}", string.Empty).Trim();
                    var nnc = Regex.Match(nc, @"(ch\. | chapter )?(\d+\.?\d+):?(.+)?", RegexOptions.IgnoreCase);
                    var ch = nnc.Groups[2].Value;
                    if (string.IsNullOrWhiteSpace(ch)) {
                        ch = nnc.Groups[0].Value;
                    }
                    var isNew = _newChapterHelper.IsNew(manga, ch, newDate,
                        link, openlink);
                }
                await Task.Delay(500);
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

        public string DbName => "Kissmanga";

        public bool LinkIsMatch(string link) {
            var regex = new Regex("");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://kissmanga.com/";
    }
}