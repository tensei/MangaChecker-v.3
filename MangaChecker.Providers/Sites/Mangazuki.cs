using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Mangazuki : IProvider
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Mangazuki(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
        }

        public async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            var rss = await _webParser.GetRssFeedAsync("https://yomanga.co/reader/feeds/rss");
            if (rss == null)
            {
                return;
            }
            //rss.Reverse();
            foreach (var manga in all)
            foreach (var rssItemObject in rss)
            {
                status.Invoke(manga);
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower()))
                {
                    continue;
                }
                var nc = rssItemObject.Title.ToLower().Replace($"{manga.Name.ToLower()} chapter", string.Empty).Trim();
                if (nc.Contains(" "))
                {
                    nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                }
                var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url)
        {
            //<div class="text">18 ⤵</div>
            var baserl = url;
            var imges = new List<object>();

            var html = await _webParser.GetHtmlSourceDocumentAsync(url);
            imges.AddRange(html.All.Where(i => i.LocalName == "img" && i.ClassList.Contains("img-lazy")
                                               && i.HasAttribute("src") &&
                                               i.GetAttribute("src").Contains("https://mangazuki.co/img/series/"))
                .Select(i => i.GetAttribute("src")));
            var pages = imges.Count;
            return new Tuple<List<object>, int>(imges, pages);
        }

        public async Task<object> CheckOne(object manga)
        {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url)
        {
            throw new NotImplementedException();
        }

        public string DbName => "Mangazuki";

        public bool LinkIsMatch(string link)
        {
            var regex = new Regex("^https://mangazuki.co/read/.+/[0-9]+$");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => true;
        public string LinktoSite => "https://mangazuki.co/";
    }
}