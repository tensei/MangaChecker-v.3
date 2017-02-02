using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Tomochan : ISite {
        //http://read.tomochan.today/rss
        public async Task CheckAll() {
            var all = LiteDB.GetMangasFrom(DbSettingName());
            var rss = await WebParser.GetRssFeedAsync("http://read.tomochan.today/rss");
            rss.Reverse();
            var openlink = LiteDB.GetOpenLinks();
            foreach (var manga in all) {
                foreach (var rssItemObject in rss) {
                    var isNew = NewChapterHelper.IsNew(manga, rssItemObject.Category, rssItemObject.PubDate,
                        rssItemObject.Link, openlink);
                    await Task.Delay(100);
                }
            }

        }

        public async Task<Manga> CheckOne(Manga manga) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<Manga> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbSettingName() {
            return "Tomochan";
        }

        public Regex LinkRegex() {
            return new Regex("");
        }
    }
}
