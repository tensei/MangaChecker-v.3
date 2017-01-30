using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using AngleSharp.Parser.Xml;
using CloudFlareUtilities;

namespace MangaChecker.Utilities {
    public class WebParser {
        private static async Task<string> GetHtmlSourceStringAsync(string url) {
            try {
                // Create the clearance handler.
                var handler = new ClearanceHandler {
                    MaxRetries = 2 // Optionally specify the number of retries, if clearance fails (default is 3).
                };

                // Create a HttpClient that uses the handler to bypass CloudFlare's JavaScript challange.
                var client = new HttpClient(handler);

                // Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
                var content = await client.GetStringAsync(url);

                return content;
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException) {
                // After all retries, clearance still failed.
                return null;
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException) {
                // Looks like we ran into a timeout. Too many clearance attempts?
                // Maybe you should increase client.Timeout as each attempt will take about five seconds.
                return null;
            }
        }

        public static async Task<IHtmlDocument> GetHtmlSourceDucumentAsync(string url, bool js = false) {
            try {
                // Create the clearance handler.
                var handler = new ClearanceHandler {
                    MaxRetries = 2 // Optionally specify the number of retries, if clearance fails (default is 3).
                };

                // Create a HttpClient that uses the handler to bypass CloudFlare's JavaScript challange.
                var client = new HttpClient(handler);

                // Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
                var content = await client.GetStringAsync(url);
                IHtmlDocument document;
                HtmlParser parser;
                if (js) {
                    //We require a custom configuration
                    var config = Configuration.Default.WithJavaScript();
                    //Let's create a new parser using this configuration
                    parser = new HtmlParser(config);
                    document = await parser.ParseAsync(content);
                    return document;
                }
                parser = new HtmlParser();
                document = await parser.ParseAsync(content);
                return document;
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException) {
                // After all retries, clearance still failed.
                return null;
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException) {
                // Looks like we ran into a timeout. Too many clearance attempts?
                // Maybe you should increase client.Timeout as each attempt will take about five seconds.
                return null;
            }
        }

        public static async Task<List<RssItemObject>> GetRssFeedAsync(string url) {
            try {
                var allXml = await GetHtmlSourceStringAsync(url);

                var parser = new XmlParser(new XmlParserOptions {
                    IsSuppressingErrors = true
                });
                var xml = await parser.ParseAsync(allXml);

                var ot = xml.All.Where(i => i.LocalName == "item");
                var list = ot.Select(element => new RssItemObject {
                    Title = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "title")?.TextContent,
                    Link = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "link")?.TextContent,
                    Category = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "category")?.TextContent,
                    Description =
                        element.Children.FirstOrDefault(x => x.TagName.ToLower() == "description")?.TextContent,
                    Guid = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "guid")?.TextContent,
                    Author = element.Children.FirstOrDefault(x => x.TagName.ToLower() == "author")?.TextContent,
                    PubDate =
                        DateTime.Parse(
                            element.Children.FirstOrDefault(x => x.TagName.ToLower() == "pubdate")?.TextContent)
                }).ToList();

                return list;
            }
            catch (Exception) {
                return null;
            }
        }
    }
}