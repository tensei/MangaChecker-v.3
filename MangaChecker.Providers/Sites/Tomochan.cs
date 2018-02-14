using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Tomochan : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Tomochan(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            DbName = nameof(Tomochan);
            LinktoSite = "http://read.tomochan.today/";
        }

        //http://read.tomochan.today/rss
        public override async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var rss = await _webParser.GetRssFeedAsync("http://read.tomochan.today/rss");
            if (rss == null)
            {
                return;
            }
            rss.Reverse();
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss)
            {
                status.Invoke(manga);
                var isNew = _newChapterHelper.IsNew(manga, rssItemObject.Category, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
                await Task.Delay(100);
            }
        }
    }
}