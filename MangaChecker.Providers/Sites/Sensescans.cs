﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Sensescans : IProvider
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Sensescans(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
        }

        public async Task CheckAll(Action<IManga> status)
        {
            // /en/0/87/5/ == 87.5
            // /en/0/24/ == 24
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            var rss = await _webParser.GetRssFeedAsync("http://sensescans.com/reader/feeds/rss/");
            if (rss == null)
            {
                return;
            }
            rss.Reverse();
            foreach (var manga in all)
            foreach (var rssItemObject in rss)
            {
                status.Invoke(manga);
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower()))
                {
                    continue;
                }
                var ncs = rssItemObject.Title.ToLower().Replace(manga.Name.ToLower(), string.Empty);
                var nc = Regex.Match(ncs.Trim(),
                    @"(?<vol>vol\.\d+)? ?(?<chapter>(chapter)?[0-9\.]+)(.+)?:?(?<chaptername>.+)?",
                    RegexOptions.IgnoreCase).Groups["chapter"].Value.Trim('.').Trim();
                if (string.IsNullOrEmpty(nc))
                {
                    nc = ncs;
                }
                var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<object> CheckOne(object manga)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url)
        {
            //<div class="text">18 ⤵</div>
            var baserl = url;
            var imges = new List<object>();
            if (!url.EndsWith("page/1"))
            {
                url = url + "page/1";
            }
            var html = await _webParser.GetHtmlSourceDocumentAsync(url);
            imges.Add(html.All.First(i => i.LocalName == "img" && i.ClassList.Contains("open")
                                          && i.HasAttribute("src") &&
                                          i.GetAttribute("src")
                                              .Contains("http://sensescans.com/reader/content/comics/"))
                .GetAttribute("src"));
            var pages =
                Regex.Match(html.DocumentElement.InnerHtml, @">([0-9]+) ⤵</div>", RegexOptions.IgnoreCase).Groups[1]
                    .Value.Trim().Trim('.');
            var intpages = int.Parse(pages);
            for (var i = 2; i <= intpages; i++)
            {
                url = baserl + $"page/{i}";
                html = await _webParser.GetHtmlSourceDocumentAsync(url);
                imges.Add(html.All.First(x => x.LocalName == "img" && x.ClassList.Contains("open")
                                              && x.HasAttribute("src") &&
                                              x.GetAttribute("src")
                                                  .Contains("http://sensescans.com/reader/content/comics/"))
                    .GetAttribute("src"));
            }
            return new Tuple<List<object>, int>(imges, intpages);
        }

        public async Task<object> FindMangaInfoOnSite(string url)
        {
            throw new NotImplementedException();
        }

        public bool LinkIsMatch(string link)
        {
            var regex = new Regex("^http://sensescans.com/reader/read/.+/en/[0-9]+/[0-9]+/?[0-9]+?/$");
            return regex.IsMatch(link);
        }

        public string DbName => "Sensescans";

        public bool ViewEnabled => true;
        public string LinktoSite => "http://sensescans.com/reader/";
    }
}