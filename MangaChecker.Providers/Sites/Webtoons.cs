using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Webtoons : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Webtoons(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            DbName = nameof(Webtoons);
            LinktoSite = "http://www.webtoons.com/";
        }

        public override async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all)
            {
                status.Invoke(manga);
                var rss = await _webParser.GetRssFeedAsync(manga.Rss);
                if (rss == null)
                {
                    continue;
                }
                rss.Reverse();
                foreach (var rssItemObject in rss)
                {
                    var title = rssItemObject.Title;
                    var nc =
                        Regex.Match(title, @"(?<other>ep\.|episode ) ?(?<chapter>\d+.?\d+|[0-9])",
                            RegexOptions.IgnoreCase);
                    var ch = nc.Groups["chapter"].Value;
                    if (string.IsNullOrWhiteSpace(ch))
                    {
                        ch = rssItemObject.Title;
                    }
                    var isNew = _newChapterHelper.IsNew(manga, ch.Trim('.').Trim(), rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                }
            }
        }
    }
}