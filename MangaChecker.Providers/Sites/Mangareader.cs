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
    public class Mangareader : IProvider {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Mangareader(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
        }

        public async Task CheckAll(Action<IManga> status) {
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all) {
                status.Invoke(manga);
                if (string.IsNullOrEmpty(manga.BaseMangaLink)) {
                    continue;
                }
                var html = await _webParser.GetHtmlSourceDocumentAsync(manga.BaseMangaLink.TrimEnd('/'));
                if (html == null) {
                    continue;
                }
                var tr =
                    html.All.Where(
                        t => t.LocalName == "tr" && t.Children.Length == 2 &&
                             t.Children[0].InnerHtml.Contains("chico"));
                foreach (var element in tr) {
                    var title = element.Children[0].Children[1].TextContent.Trim();
                    if (title.Contains("Chapter Name")) {
                        continue;
                    }
                    var newDate = DateTime.Parse(element.Children[1].TextContent.Trim('\n').Trim(),
                        CultureInfo.InvariantCulture);
                    var link = "http://mangareader.net" + element.Children[0].Children[1].GetAttribute("href");
                    if (!title.ToLower().Contains(manga.Name.ToLower())) {
                        continue;
                    }
                    var nc =
                        title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                    if (nc.Contains(" ")) {
                        nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    var isNew = _newChapterHelper.IsNew(manga, nc, newDate,
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

        public string DbName => "Mangareader";

        public bool LinkIsMatch(string link) {
            var regex = new Regex("");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://www.mangareader.net/";
    }
}