using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Mangadex : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Mangadex(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper, Logger logger)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            _logger = logger;
            DbName = nameof(Mangadex);
            ViewEnabled = false;
            LinktoSite = "https://mangadex.org/";
        }

        public override async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName).ToList();
            var b = _dbContext.GetMangasFrom("Batoto").ToList();
            if (b.Any())
            {
                foreach (var manga in b)
                {
                    manga.Site = nameof(Mangadex);
                }
                _dbContext.UpdateMangaTrans(b);
                all.AddRange(b);
            }
            var brss = _dbContext.GetSettingsFor("Mangadex Rss");
            var rss = await _webParser.GetRssFeedAsync(brss.Link);
            if (rss == null)
            {
                return;
            }
            rss.Reverse();
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss)
            {
                // example title Jitsu wa Watashi wa - English - Vol.18 Ch.156: Aizawa Nagisa and Aizawa Nagisa②
                status(manga);
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower()))
                {
                    continue;
                }
                var ncn = Regex.Match(rssItemObject.Title,
                    @" Chapter (?<chapter>[0-9\.]+)",
                    RegexOptions.IgnoreCase);
                var ch = ncn.Groups["chapter"].Value.Trim();
                if (string.IsNullOrEmpty(ch))
                {
                    _logger.Log.Warn($"var ch={ch}, var Rss title={rssItemObject.Title}");
                    continue;
                }
                var isNew = _newChapterHelper.IsNew(manga, ch.Trim('.'), rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }
    }
}