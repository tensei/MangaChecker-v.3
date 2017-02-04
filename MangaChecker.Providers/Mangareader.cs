﻿using System;
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
    public class Mangareader : ISite {
        public async Task CheckAll()
        {
            var all = LiteDB.GetMangasFrom(DbSettingName());
            var openlink = LiteDB.GetOpenLinks();
            foreach (var manga in all) {
                if(string.IsNullOrEmpty(manga.BaseMangaLink)) continue;
                var html = await WebParser.GetHtmlSourceDucumentAsync(manga.BaseMangaLink.TrimEnd('/'));
                if (html == null)
                    continue;
                var tr = html.All.Where(t => t.LocalName == "tr" && t.Children.Length == 2 && t.Children[0].InnerHtml.Contains("chico"));
                foreach (var element in tr) {
                    var title = element.Children[0].Children[1].TextContent.Trim();
                    if (title.Contains("Chapter Name"))
                        continue;
                    var newDate = DateTime.Parse(element.Children[1].TextContent.Trim('\n').Trim(), CultureInfo.InvariantCulture);
                    var link = "http://mangareader.net" + element.Children[0].Children[1].GetAttribute("href");
                    if (!title.ToLower().Contains(manga.Name.ToLower()))
                        continue;
                    var nc =
                        title.ToLower().Replace(manga.Name.ToLower(), string.Empty).Trim();
                    if (nc.Contains(" "))
                        nc = nc.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var isNew = NewChapterHelper.IsNew(manga, nc, newDate,
                        link, openlink);
                }
                await Task.Delay(500);
            }
        }

        public async Task<IEnumerable<object>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<Manga> CheckOne(Manga manga) {
            throw new NotImplementedException();
        }

        public async Task<Manga> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbSettingName() {
            return "Mangareader";
        }

        public Regex LinkRegex() {
            return new Regex("");
        }
    }
}