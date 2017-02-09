using System;
using MangaChecker.Data.Model;

namespace MangaChecker.Data.Interface {
    public interface INewChapterHelper {
        bool IsNew(Manga manga, string newChapter, DateTime newDate, string newLink, bool openLink, ILiteDb liteDb);

        bool Update(Manga manga, float floatChapter, bool isFloat, string newLink, DateTime newDate,
            bool openLink, object newChapter, ILiteDb liteDb);
    }
}