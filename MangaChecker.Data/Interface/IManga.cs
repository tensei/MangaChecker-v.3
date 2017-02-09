using System;
using System.Collections.Generic;
using MangaChecker.Data.Enum;

namespace MangaChecker.Data.Interface {
    public interface IManga {
        int MangaId { get; set; }
        string Name { get; set; }
        float Chapter { get; set; }
        object Newest { get; set; }
        bool New { get; set; }
        List<string> OtherChapters { get; set; }
        string Site { get; set; }
        string Link { get; set; }
        string BaseMangaLink { get; set; }
        string Rss { get; set; }
        string SpecialLink { get; set; }
        string Thumbnail { get; set; }
        DateTime Added { get; set; }
        List<Genre> Genres { get; set; }
        DateTime Updated { get; set; }
        string DaysAgo { get; }
    }
}