﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Batoto : IProvider
    {
        private readonly IDbContext _dbContext;
        private readonly Logger _logger;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Batoto(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper, Logger logger)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            _logger = logger;
        }

        public async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var brss = _dbContext.GetSettingsFor("Batoto Rss");
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
                status.Invoke(manga);
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower()))
                {
                    continue;
                }
                var ncn = Regex.Match(rssItemObject.Title,
                    @"(?<manga>.+) - .+ - ?([vol]+\.[0-9\.]+)? [ch\.]+?(?<chapter>[0-9\.]+)(\(.+\))?:?(?<chaptername>.+)",
                    RegexOptions.IgnoreCase);
                var ch = ncn.Groups["chapter"]?.Value.Trim() ?? ncn.Groups["chaptername"]?.Value.Trim();
                if (string.IsNullOrEmpty(ch))
                {
                    _logger.Log.Warn($"var ch={ch}, var Rss title={rssItemObject.Title}");
                    continue;
                }
                var isNew = _newChapterHelper.IsNew(manga, ch.Trim('.'), rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<object> CheckOne(object manga)
        {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url)
        {
            throw new NotImplementedException();
        }

        public string DbName => "Batoto";

        public bool LinkIsMatch(string link)
        {
            var regex = new Regex("");
            return regex.IsMatch(link);
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://bato.to/";
    }
}