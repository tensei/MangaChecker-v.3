using System;
using System.Diagnostics;
using System.Globalization;
using MangaChecker.Data.Interfaces;
using MangaChecker.Data.Models;
using MangaChecker.Database;
using static MangaChecker.Utilities.Logger;

namespace MangaChecker.Utilities {
    public class NewChapterHelper : INewChapterHelper {
        private readonly ILiteDb _liteDb;
        private readonly Logger _logger;
        public NewChapterHelper(ILiteDb liteDb, Logger logger) {
            _liteDb = liteDb;
            _logger = logger;
        }
        public bool IsNew(Manga manga, string newChapter, DateTime newDate, string newLink, bool openLink) {
            var isFloat = float.TryParse(newChapter, NumberStyles.Float, CultureInfo.InvariantCulture,
                out float floatChapter);
            var isDateNew = newDate > manga.Updated;

            if (newChapter.StartsWith("Bonus")) {
                Console.WriteLine("hmmmmm");
            }
            if (isFloat && Math.Abs(floatChapter - manga.Chapter) <= 0  || newChapter == manga.Newest || Math.Abs(floatChapter) > 0 &&
                floatChapter < manga.Chapter) {
                return true;
            }

            if (isFloat && floatChapter > manga.Chapter) {
                return Update(manga, floatChapter, true, newLink, newDate, openLink, newChapter);
            }
            if (!isFloat && isDateNew && !manga.OtherChapters.Contains(newChapter)) {
                manga.OtherChapters.Add(newChapter);
                return Update(manga, floatChapter, false, newLink, newDate, openLink, newChapter);
            }
            //this should never be reached!!
            _logger.Log.Error($"Current manga.Name={manga.Name}," +
                      $" manga.Chapter={manga.Chapter}," +
                      $" manga.Newest={manga.Newest}, " +
                      $" manga.Link={manga.Link}, " +
                      $" manga.Rss={manga.Rss},\n" +
                      $"newChapter={newChapter}, floatChapter={floatChapter}, newLink={newLink}, newDate={newDate}");
            return false;
        }

        public bool Update(Manga manga, float floatChapter, bool isFloat, string newLink, DateTime newDate,
            bool openLink, object newChapter) {
            manga.Newest = newChapter;
            if (isFloat) {
                manga.Chapter = floatChapter;
                manga.Newest = floatChapter.ToString(CultureInfo.InvariantCulture).Contains(".")
                    ? floatChapter.ToString("0.00", CultureInfo.InvariantCulture)
                    : floatChapter.ToString(CultureInfo.InvariantCulture);
            }
            manga.Updated = newDate;
            manga.Link = newLink;
            manga.New = true;
            if (!openLink) {
                return true;
            }
            _liteDb.Update(manga);
            Process.Start(newLink);
            return true;
        }
    }
}