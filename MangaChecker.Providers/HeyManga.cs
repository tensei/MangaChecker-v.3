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
    public class HeyManga : ISite {
        public async Task CheckAll() {
            var all = LiteDB.GetMangasFrom(DbName);
            var openlink = LiteDB.GetOpenLinks();
            var m = await _load2Pages("https://www.heymanga.me/latest-manga/");
            if (m == null) return;
            var m2 = await _load2Pages("https://www.heymanga.me/latest-manga/2");
            if (m2 != null) {
                foreach (var d in m2) {
                    if (m.Keys.Contains(d.Key)) continue;
                    m.Add(d.Key, d.Value);
                }
            }
            foreach (var manga in all)
            foreach (var rssItemObject in m) {
                if (!rssItemObject.Value.ToLower().Contains(manga.Name.ToLower())) continue;
                var nc = rssItemObject.Value.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                if (nc.Contains(" ")) nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                var isNew = NewChapterHelper.IsNew(manga, nc, DateTime.Now,
                    rssItemObject.Key, openlink);
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

        public string DbName => "HeyManga";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;

        private async Task<Dictionary<string, string>> _load2Pages(string link) {
            var m = new Dictionary<string, string>();
            var html = await WebParser.GetHtmlSourceDucumentAsync(link);
            var a =
                html.All.Where(
                    x =>
                        x.LocalName == "a" && x.HasAttribute("href") &&
                        x.GetAttribute("href").Contains("http://www.heymanga.me/manga/"));
            foreach (var element in a.Reverse())
                try {
                    if (m.Keys.Contains(element.GetAttribute("href"))) continue;
                    m.Add(element.GetAttribute("href"), element.Children[0].InnerHtml);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            return m;
        }
    }
}