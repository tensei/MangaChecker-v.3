using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class KireiCake : ISite {
        public async Task CheckAll() {
            // /en/0/87/5/ == 87.5
            // /en/0/24/ == 24
            var all = LiteDB.GetMangasFrom(DbName);
            var openlink = LiteDB.GetOpenLinks();
            var rss = await WebParser.GetRssFeedAsync("https://reader.kireicake.com/rss.xml");
            if (rss == null) return;
            rss.Reverse();
            foreach (var manga in all)
            foreach (var rssItemObject in rss) {
                if (!rssItemObject.Title.ToLower().Contains(manga.Name.ToLower())) continue;
                var ncs = rssItemObject.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                string nc;
                if (ncs[ncs.Length - 4] == "en") nc = $"{ncs[ncs.Length - 2]}.{ncs[ncs.Length - 1]}";
                else if (ncs[ncs.Length - 3] == "en") nc = $"{ncs.Last()}";
                else continue;
                var isNew = NewChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
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

        public string DbName => "KireiCake";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
    }
}