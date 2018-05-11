using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Jaiminisbox : ProviderBase
    {
        private readonly IDbContext _dbContext;
        private readonly INewChapterHelper _newChapterHelper;
        private readonly IWebParser _webParser;

        public Jaiminisbox(IWebParser webParser, IDbContext dbContext, INewChapterHelper newChapterHelper)
        {
            _webParser = webParser;
            _dbContext = dbContext;
            _newChapterHelper = newChapterHelper;
            DbName = nameof(Jaiminisbox);
            ViewEnabled = true;
            LinktoSite = "https://jaiminisbox.com/";
        }

        public override async Task CheckAll(Action<IManga> status)
        {
            // /en/0/87/5/ == 87.5
            // /en/0/24/ == 24
            var all = _dbContext.GetMangasFrom(DbName);
            var openlink = _dbContext.GetOpenLinks();
            var rss = await _webParser.GetRssFeedAsync("https://jaiminisbox.com/reader/feeds/rss");
            if (rss == null)
            {
                return;
            }
            rss.Reverse();
            foreach (var manga in all)
            foreach (var rssItemObject in rss)
            {
                status.Invoke(manga);
                if (!rssItemObject.Title.ToLower().StartsWith(manga.Name.ToLower()))
                {
                    continue;
                }
                var ncs = rssItemObject.Link.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                string nc;
                if (ncs[ncs.Length - 4] == "en")
                {
                    nc = $"{ncs[ncs.Length - 2]}.{ncs[ncs.Length - 1]}";
                }
                else if (ncs[ncs.Length - 3] == "en")
                {
                    nc = $"{ncs.Last()}";
                }
                else
                {
                    continue;
                }
                var isNew = _newChapterHelper.IsNew(manga, nc, rssItemObject.PubDate,
                    rssItemObject.Link, openlink);
            }
        }

        public override async Task<(List<object>, int)> GetImagesTaskAsync(string url)
        {
            //<div class="text">18 ⤵</div>
            var baserl = url;
            var imges = new List<object>();
            if (!url.EndsWith("page/1"))
            {
                url = url + "page/1";
            }
            var html = await _webParser.GetHtmlSourceDocumentAsync(url, true);
            imges.Add(html.All.First(i => i.LocalName == "img" && i.ClassList.Contains("open")
                                          && i.HasAttribute("src"))
                .GetAttribute("src"));
            var pages =
                Regex.Match(html.DocumentElement.InnerHtml, @">([0-9]+) ⤵</div>", RegexOptions.IgnoreCase).Groups[1]
                    .Value.Trim().Trim('.');
            var intpages = int.Parse(pages);
            for (var i = 2; i <= intpages; i++)
            {
                url = baserl + $"page/{i}";
                html = await _webParser.GetHtmlSourceDocumentAsync(url);
                imges.Add(html.All.First(x => x.LocalName == "img" && x.ClassList.Contains("open")
                                              && x.HasAttribute("src"))
                    .GetAttribute("src"));
            }
            return (imges, intpages);
        }
        

        public override bool LinkIsMatch(string link)
        {
            var regex = new Regex(@"^https?://jaiminisbox.com/reader/read/.+/en/\d+/\d+/(\d+/)?$");
            return regex.IsMatch(link);
        }

    }
}