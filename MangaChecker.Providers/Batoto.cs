﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interface;
using MangaChecker.Database;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Batoto : IProvider {
        private readonly WebParser _webParser = new WebParser();

        public async Task CheckAll() {
            var all = LiteDb.GetMangasFrom(DbName);
            var brss = LiteDb.GetSettingsFor("Batoto Rss");
            var rss = await _webParser.GetRssFeedAsync(brss.Link);
            if (rss == null) {
                return;
            }
            rss.Reverse();
            var openlink = LiteDb.GetOpenLinks();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                // example title Jitsu wa Watashi wa - English - Vol.18 Ch.156: Aizawa Nagisa and Aizawa Nagisa②
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) {
                    continue;
                }
                var ncn = Regex.Match(rssItemObject.Title,
                    @"(?<manga>.+) - .+ - ?([vol]+\.[0-9\.]+)? [ch\.]+?(?<chapter>[0-9\.]+)(\(.+\))?:?(?<chaptername>.+)",
                    RegexOptions.IgnoreCase);
                var ch = ncn.Groups["chapter"]?.Value.Trim() ?? ncn.Groups["chaptername"]?.Value.Trim();
                if (string.IsNullOrEmpty(ch)) {
                    Logger.Log.Warn($"var ch={ch}, var Rss title={rssItemObject.Title}");
                    continue;
                }
                var isNew = NewChapterHelper.IsNew(manga, ch.Trim('.'), rssItemObject.PubDate,
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

        public string DbName => "Batoto";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://bato.to/";
    }
}