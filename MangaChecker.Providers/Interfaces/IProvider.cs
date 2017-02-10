using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MangaChecker.Providers.Interfaces {
    public interface IProvider {
        string DbName { get; }
        bool ViewEnabled { get; }
        string LinktoSite { get; }
        Regex LinkRegex();
        Task CheckAll();
        Task<object> CheckOne(object manga);
        Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url);
        Task<object> FindMangaInfoOnSite(string url);
    }
}