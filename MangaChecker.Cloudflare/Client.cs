using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CloudFlareUtilities;

namespace MangaChecker.Cloudflare
{
	public class Client {
		public async Task<string> GetHtmlSource(string url) {
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

			} catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException) {
				// After all retries, clearance still failed.
				return null;
			} catch (AggregateException ex) when (ex.InnerException is TaskCanceledException) {
				// Looks like we ran into a timeout. Too many clearance attempts?
				// Maybe you should increase client.Timeout as each attempt will take about five seconds.
				return null;
			}
		}

		public async Task<SyndicationFeed> GetRssFeed(string url) {
			try {
				var allXml= await GetHtmlSource(url);
				var bytes = Encoding.Default.GetBytes(allXml);
				allXml = Encoding.UTF8.GetString(bytes);
				SyndicationFeed feed;
				try {
					var xmlr = XmlReader.Create(new StringReader(allXml));
					feed = SyndicationFeed.Load(xmlr);
				} catch (Exception) {
					allXml = allXml.Replace("pubDate", "fuck").Replace("&#45;", "-")
						.Replace("lastBuildDate", "fuck2");
					allXml = Regex.Replace(allXml, "<img src=\".+\"  />", "fuck");
					var xmlr = XmlReader.Create(new StringReader(allXml));
					feed = SyndicationFeed.Load(xmlr);
				}
				return feed;
			} catch (Exception) {
				return null;
			}
		}
	}
}
