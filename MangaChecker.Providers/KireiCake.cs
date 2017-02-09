﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interface;
using MangaChecker.Database;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class KireiCake : IProvider {
        private readonly WebParser _webParser = new WebParser();

        public async Task CheckAll() {
            // /en/0/87/5/ == 87.5
            // /en/0/24/ == 24
            var all = LiteDb.GetMangasFrom(DbName);
            var openlink = LiteDb.GetOpenLinks();
            var rss = await _webParser.GetRssFeedAsync("https://reader.kireicake.com/rss.xml");
            if (rss == null) {
                return;
            }
            rss.Reverse();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) {
                    continue;
                }
                var ncs = rssItemObject.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                string nc;
                if (ncs[ncs.Length - 4] == "en") {
                    nc = $"{ncs[ncs.Length - 2]}.{ncs[ncs.Length - 1]}";
                }
                else if (ncs[ncs.Length - 3] == "en") {
                    nc = $"{ncs.Last()}";
                }
                else {
                    continue;
                }
                var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            //<div class="text">18 ⤵</div>
            var baserl = url;
            var imges = new List<object>();
            if (!url.EndsWith("page/1")) {
                url = url + "page/1";
            }
            var html = await _webParser.GetHtmlSourceDucumentAsync(url);
            imges.Add(html.All.First(i => i.LocalName == "img" && i.ClassList.Contains("open")
                                          && i.HasAttribute("src") &&
                                          i.GetAttribute("src").Contains("https://reader.kireicake.com/content/comics/"))
                .GetAttribute("src"));
            var pages =
                Regex.Match(html.DocumentElement.InnerHtml, @">([0-9]+) ⤵</div>", RegexOptions.IgnoreCase).Groups[1]
                    .Value.Trim().Trim('.');
            var intpages = int.Parse(pages);
            for (var i = 2; i <= intpages; i++) {
                url = baserl + $"page/{i}";
                html = await _webParser.GetHtmlSourceDucumentAsync(url);
                imges.Add(html.All.First(x => x.LocalName == "img" && x.ClassList.Contains("open")
                                              && x.HasAttribute("src") &&
                                              x.GetAttribute("src")
                                                  .Contains("https://reader.kireicake.com/content/comics/"))
                    .GetAttribute("src"));
            }
            return new Tuple<List<object>, int>(imges, intpages);
        }

        public async Task<object> CheckOne(object manga) {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbName => "KireiCake";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => true;
        public string LinktoSite => "http://kireicake.com/";
    }
}