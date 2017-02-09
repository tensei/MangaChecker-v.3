using System;
using System.Diagnostics;
using System.Globalization;
using MangaChecker.Data.Interface;
using MangaChecker.Data.Model;
using MangaChecker.Database;
using static MangaChecker.Utilities.Logger;

namespace MangaChecker.Utilities {
    public class NewChapterHelper : INewChapterHelper {
        public bool IsNew(Manga manga, string newChapter, DateTime newDate, string newLink, bool openLink, ILiteDb liteDb) {
            var isFloat = float.TryParse(newChapter, NumberStyles.Float, CultureInfo.InvariantCulture,
                out float floatChapter);
            var isDateNew = newDate > manga.Updated;

            if (isFloat && Math.Abs(floatChapter - manga.Chapter) <= 0 || newChapter == manga.Newest ||
                floatChapter < manga.Chapter) {
                return true;
            }

            if (isFloat && floatChapter > manga.Chapter) {
                return Update(manga, floatChapter, true, newLink, newDate, openLink, newChapter, liteDb);
            }

            if (isDateNew && !manga.OtherChapters.Contains(newChapter)) {
                manga.OtherChapters.Add(newChapter);
                return Update(manga, floatChapter, isFloat, newLink, newDate, openLink, newChapter, liteDb);
            }
            //this should never be reached!!
            Log.Error($"Current manga.Name={manga.Name}," +
                      $" manga.Chapter={manga.Chapter}," +
                      $" manga.Newest={manga.Newest}, " +
                      $" manga.Link={manga.Link}, " +
                      $" manga.Rss={manga.Rss},\n" +
                      $"newChapter={newChapter}, floatChapter={floatChapter}, newLink={newLink}, newDate={newDate}");
            return false;
        }

        public bool Update(Manga manga, float floatChapter, bool isFloat, string newLink, DateTime newDate,
            bool openLink, object newChapter, ILiteDb liteDb) {
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
            liteDb.Update(manga);
            Process.Start(newLink);
            return true;
        }
    }
}