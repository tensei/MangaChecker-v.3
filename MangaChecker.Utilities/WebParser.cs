using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using AngleSharp.Parser.Xml;
using CloudFlareUtilities;
using MangaChecker.Data.Models;
using MangaChecker.Utilities.Interfaces;

namespace MangaChecker.Utilities
{
    public class WebParser : IWebParser
    {
        private readonly HttpClient _client;

        public WebParser()
        {
            var handler = new ClearanceHandler
            {
                MaxRetries = 2 // Optionally specify the number of retries, if clearance fails (default is 3).
            };
            _client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            _client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
            _client.DefaultRequestHeaders.Add("DNT", "1");
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
        }

        public async Task<string> GetHtmlSourceStringAsync(string url)
        {
            try
            {
                // Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsByteArrayAsync();

                var responseString = Encoding.UTF8.GetString(content, 0, content.Length);

                return responseString.Replace("&#45;", "-");
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException)
            {
                // After all retries, clearance still failed.
                return null;
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                // Looks like we ran into a timeout. Too many clearance attempts?
                // Maybe you should increase client.Timeout as each attempt will take about five seconds.
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]> GetHtmlDataAsync(string url)
        {
            try
            {
                // Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException)
            {
                // After all retries, clearance still failed.
                return null;
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                // Looks like we ran into a timeout. Too many clearance attempts?
                // Maybe you should increase client.Timeout as each attempt will take about five seconds.
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IHtmlDocument> GetHtmlSourceDocumentAsync(string url, bool js = false)
        {
            // Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
            string content;
            try
            {
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                content = await response.Content.ReadAsStringAsync();
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException)
            {
                // After all retries, clearance still failed.
                return null;
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                // Looks like we ran into a timeout. Too many clearance attempts?
                // Maybe you should increase client.Timeout as each attempt will take about five seconds.
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            // Use javascript parser on js true
            var parser = js ? new HtmlParser(Configuration.Default.WithJavaScript()) : new HtmlParser();
            var document = await parser.ParseAsync(content);
            return document;
        }

        public async Task<List<RssItem>> GetRssFeedAsync(string url)
        {
            try
            {
                var allXml = await GetHtmlSourceStringAsync(url);
                if (allXml == null)
                {
                    return null;
                }
                var parser = new XmlParser(new XmlParserOptions
                {
                    IsSuppressingErrors = true
                });
                var xml = await parser.ParseAsync(allXml);
                var l = new List<RssItem>();
                var ot = xml.All.Where(i => i.LocalName == "item" || i.LocalName == "entry");
                var enumerable = ot as IList<IElement> ?? ot.ToList();
                foreach (var element in enumerable.ToList())
                {
                    var title = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "title")?.TextContent;
                    var id = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "id")?.TextContent;
                    var link = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "link")?.TextContent;
                    var category = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "category")?.TextContent;
                    var description =
                        element.Children.FirstOrDefault(x => x.TagName.ToLower() == "description")?.TextContent;
                    var guid = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "guid")?.TextContent;
                    var author = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "author")?.TextContent;
                    var pubDate =
                        DateTime.TryParse(element.Children.FirstOrDefault(x => x.TagName.ToLower() == "pubdate")?.TextContent, out DateTime pd);
                    l.Add(new RssItem
                    {
                        Id = id,
                        Title = title,
                        Link = link,
                        Category = category,
                        Description = description,
                        Guid = guid,
                        Author = author,
                        PubDate =  pubDate ? pd : DateTime.Now
                    });
                }
//                var list = enumerable.Select(element => new RssItem
//                {
//                    Title = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "title")?.TextContent,
//                    Link = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "link")?.TextContent,
//                    Category = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "category")?.TextContent,
//                    Description =
//                        element.Children.FirstOrDefault(x => x.TagName.ToLower() == "description")?.TextContent,
//                    Guid = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "guid")?.TextContent,
//                    Author = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "author")?.TextContent,
//                    PubDate =
//                        DateTime.Parse(
//                            element.Children.FirstOrDefault(x => x.TagName.ToLower() == "pubdate")?.TextContent)
//                }).ToList();
                return l;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}