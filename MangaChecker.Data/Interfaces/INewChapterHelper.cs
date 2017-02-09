using System;
using MangaChecker.Data.Models;

namespace MangaChecker.Data.Interfaces {
    public interface INewChapterHelper {
        bool IsNew(Manga manga, string newChapter, DateTime newDate, string newLink, bool openLink);
    }
}