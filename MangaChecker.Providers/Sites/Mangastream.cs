﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites {
    public class Mangastream : IProvider {
        private readonly IWebParser _webParser;
        private readonly ILiteDb _liteDb;
        private readonly INewChapterHelper _newChapterHelper;

        public Mangastream(IWebParser webParser, ILiteDb liteDb, INewChapterHelper newChapterHelper) {
            _webParser = webParser;
            _liteDb = liteDb;
            _newChapterHelper = newChapterHelper;
        }
        public async Task CheckAll() {
            var all = _liteDb.GetMangasFrom(DbName);
            var rss = await _webParser.GetRssFeedAsync("http://mangastream.com/rss");
            if (rss == null) {
                return;
            }
            rss.Reverse();
            var openlink = _liteDb.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) {
                    continue;
                }
                var nc = rssItemObject.Title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
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

        public string DbName => "Mangastream";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://mangastream.com/";
    }
}