using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;

namespace MangaChecker.DataTypes.Interface {
    public interface ISite {
        Regex LinkRegex();
        string DbName { get; }
        Task CheckAll();
        Task<Manga> CheckOne(Manga manga);
        Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url);
        Task<Manga> FindMangaInfoOnSite(string url);
        bool ViewEnabled { get; }
    }
}