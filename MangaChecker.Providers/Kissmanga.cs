using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;
using MangaChecker.Utilities;

namespace MangaChecker.Providers {
    public class Kissmanga : ISite {
        public async Task CheckAll() {
            var all = LiteDb.GetMangasFrom(DbName);
            var openlink = LiteDb.GetOpenLinks();
            foreach (var manga in all) {
                var html = await WebParser.GetHtmlSourceDucumentAsync(manga.BaseMangaLink);
                if(html == null) continue;
                var tr = html.All.Where(t => t.LocalName == "tr" && t.Children.Length == 2);
                foreach (var element in tr) {
                    var title = element.Children[0].TextContent.Trim();
                    if(title.Contains("Chapter Name")) continue;
                    var newDate = DateTime.Parse(element.Children[1].TextContent.Trim('\n').Trim(), CultureInfo.InvariantCulture);
                    var link = "http://kissmanga.com"+element.Children[0].Children[0].GetAttribute("href");
                    if (!title.ToLower().Contains(manga.Name.ToLower())) continue;
                    var nc =
                        title.ToLower().Replace($"{manga.Name.ToLower()} chapter", string.Empty).Trim();
                    if (nc.Contains(" ")) nc = nc.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                    var isNew = NewChapterHelper.IsNew(manga, nc, newDate,
                        link, openlink);
                }
                await Task.Delay(500);
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

        public string DbName => "Kissmanga";

        public Regex LinkRegex() {
            return new Regex("");
        }

        public bool ViewEnabled => false;
        public string LinktoSite => "http://kissmanga.com/";
    }
}