﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Kissmanga : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Kissmanga(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            DbName = nameof(Kissmanga);
            LinktoSite = "http://kissmanga.com/";
        }

        public override async Task CheckAll(Action<IManga> status)
        {
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            foreach (var manga in all)
            {
                status.Invoke(manga);
                var html = await _webParser.GetHtmlSourceDocumentAsync(manga.BaseMangaLink);
                if (html == null)
                {
                    continue;
                }
                var tr = html.All.Where(t => t.LocalName == "tr" && t.Children.Length == 2);
                foreach (var element in tr.Reverse())
                {
                    var title = element.Children[0].TextContent.Trim();
                    if (title.Contains("Chapter Name"))
                    {
                        continue;
                    }
                    var newDate = DateTime.Parse(element.Children[1].TextContent.Trim('\n').Trim(),
                        CultureInfo.InvariantCulture);
                    var link = "http://kissmanga.com" + element.Children[0].Children[0].GetAttribute("href");
                    if (!title.ToLower().Contains(manga.Name.ToLower()))
                    {
                        continue;
                    }
                    var nc =
                        title.ToLower().Replace($"{manga.Name.ToLower()}", string.Empty).Trim();
                    var nnc = Regex.Match(nc, @".*(ch\. | chapter )?(\d+\.?\d+):?.*", RegexOptions.IgnoreCase);
                    var ch = nnc.Groups[2].Value;
                    if (string.IsNullOrWhiteSpace(ch))
                    {
                        ch = nnc.Groups[0].Value;
                    }
                    var isNew = _newChapterHelper.IsNew(manga, ch, newDate,
                        link, openlink);
                }
                await Task.Delay(500);
            }
        }
        
    }
}