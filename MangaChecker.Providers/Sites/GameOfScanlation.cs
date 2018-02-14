using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class GameOfScanlation : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public GameOfScanlation(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            DbName = nameof(GameOfScanlation);
            LinktoSite = "https://gameofscanlation.moe/";
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
                    var nc =
                        rssItemObject.Title.ToLower().Replace($"{manga.Name.ToLower()} chapter", string.Empty).Trim();
                    if (nc.Contains(" "))
                    {
                        nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                }
            }
        }
        
        
    }
}