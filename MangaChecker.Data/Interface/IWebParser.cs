using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using MangaChecker.Data.Model;

namespace MangaChecker.Data.Interface {
    public interface IWebParser {
        Task<string> GetHtmlSourceStringAsync(string url);
        Task<byte[]> GetHtmlDataAsync(string url);
        Task<IHtmlDocument> GetHtmlSourceDucumentAsync(string url, bool js = false);
        Task<List<RssItem>> GetRssFeedAsync(string url);
    }
}