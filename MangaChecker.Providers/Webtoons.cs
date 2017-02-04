﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Webtoons : ISite {
        public async Task CheckAll() {
            var all = LiteDB.GetMangasFrom(DbName);
            var openlink = LiteDB.GetOpenLinks();
            foreach (var manga in all) {
                var rss = await WebParser.GetRssFeedAsync(manga.Rss);
                if (rss == null) continue;
                rss.Reverse();
                foreach (var rssItemObject in rss) {
                    var nc = rssItemObject.Title.Replace("Ep. ", string.Empty);
                    var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                }
            }
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<Manga> CheckOne(Manga manga) {
            throw new NotImplementedException();
        }

        public async Task<Manga> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbName => "Webtoons";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
    }
}