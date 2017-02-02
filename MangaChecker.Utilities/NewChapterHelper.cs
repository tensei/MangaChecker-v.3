using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaChecker.Database.Tables;

namespace MangaChecker.Utilities {
    public static class NewChapterHelper {
        public static bool IsNew(Manga manga, string newChapter, DateTime newDate, string newLink, bool openLink) {
            var isFloat = float.TryParse(newChapter, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatChapter);
            var isDateNew = newDate > manga.Updated;
            if (isFloat && floatChapter > manga.Chapter) return Update(manga, floatChapter, true, newLink, newDate, openLink, newChapter);
            if (!isDateNew || manga.OtherChapters.Contains(newChapter)) return false;
            manga.OtherChapters.Add(newChapter);
            return Update(manga, floatChapter, isFloat, newLink, newDate, openLink, newChapter);
        }

        private static bool Update(Manga manga, float floatChapter, bool isFloat, string newLink, DateTime newDate, bool openLink, object newChapter) {
            manga.Newest = newChapter;
            if (isFloat) {
                manga.Chapter = floatChapter;
                manga.Newest = floatChapter.ToString(CultureInfo.InvariantCulture).Contains(".") ? 
                    floatChapter.ToString("0.00", CultureInfo.InvariantCulture) :
                    floatChapter.ToString(CultureInfo.InvariantCulture);
            }
            manga.Updated = newDate;
            manga.Link = newLink;
            manga.New = true;
            if (!openLink) return true;
            LiteDB.Update(manga);
            Process.Start(newLink);
            return true;
        }
    }
}
