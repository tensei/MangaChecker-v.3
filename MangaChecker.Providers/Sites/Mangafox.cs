using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Mangafox : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Mangafox(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper, Logger logger)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            _logger = logger;
            DbName = nameof(Mangafox);
            LinktoSite = "http://mangafox.me/";
        }

        public override async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all)
            {
                status.Invoke(manga);
                if (string.IsNullOrEmpty(manga.Rss))
                {
                    _logger.Log.Warn($"MANGAFOX {manga.Name} missing rss feed link");
                    continue;
                }
                var rss = await _webParser.GetRssFeedAsync(manga.Rss);
                if (rss == null)
                {
                    continue;
                }
                rss.Reverse();
                foreach (var rssItemObject in rss)
                {
                    // sample title => Boku no Hero Academia Vol TBD Ch 125
                    var re = Regex.Match(rssItemObject.Title,
                        @"(?<name>.+)(?<vol>[vol tbd0-9]+)?[ch ]+?(?<chapter>[0-9\.]+)", RegexOptions.IgnoreCase);
                    var nc = string.IsNullOrEmpty(re.Groups["chapter"].Value)
                        ? rssItemObject.Title.Replace(manga.Name.ToLower(), string.Empty)
                        : re.Groups["chapter"].Value.Trim('.').Trim();
                    var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                    await Task.Delay(100);
                }
            }
        }
    }
}