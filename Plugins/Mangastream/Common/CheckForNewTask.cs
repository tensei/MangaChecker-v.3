using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using MangaChecker.Cloudflare;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;
using Mangastream.ViewModel;

namespace Mangastream.Common {
	public class CheckForNewTask {
		public async void Run() {
			MainViewModel.Instance.Running = "YEEEEEEEEE";
			//Task.Run(async () => {
				var time = 3;
				while (true) {
					if (time > 1) {
						time--;
						await Task.Delay(1000);
						MainViewModel.Instance.Running = time.ToString();
					} else if(time <= 1) {
						var db = new Database();
						var mangas = await db.GetMangasFrom("mangastream");
						MainViewModel.Instance.Running = "hmmmmmmm";
						var feed = await new Client().GetRssFeed("http://mangastream.com/rss");
						var settings = await db.GetSettingsFor("Mangastream");
						foreach (var manga in mangas) {

							MainViewModel.Instance.Running = $"checking {manga.Name}";
							await CheckForNew(manga, feed, settings.OpenLinks);
						}
						time = await new Database().GetRefreshTime();
					}
				}
				// ReSharper disable once FunctionNeverReturns
			//});
		}

		private static async Task<bool> CheckForNew(Manga manga, SyndicationFeed feed, bool openlinks) {
			foreach (var syndicationItem in feed.Items) {
				var title = syndicationItem.Title.Text;
				var link = syndicationItem.Id;
				var matches = Regex.Match(title, @"(?<name>.+) #?(?<chapter>[0-9\.]+) ?(end)?", RegexOptions.IgnoreCase);
				if (matches.Groups.Count <= 1) {
					continue;
				}
				var chapter = float.Parse(matches.Groups["chapter"].Value);
				var name = matches.Groups["name"].Value;
				if (!title.ToLower().Contains(manga.Name.ToLower())) {
					continue;
				}
				if (chapter <= manga.Chapter) {
					continue;
				}
				//if (!openlinks) continue;
				Process.Start(link);
				manga.Chapter = chapter;
				manga.Link = link;
				manga.Updated = DateTime.Now;
				await new Database().Update(manga);
				return true;
			}
			return false;
		}
	}
}
