using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using MangaChecker.Data.Models;

namespace MangaChecker.Utilities.Interfaces {
    public interface IWebParser {
        Task<string> GetHtmlSourceStringAsync(string url);
        Task<byte[]> GetHtmlDataAsync(string url);
        Task<IHtmlDocument> GetHtmlSourceDocumentAsync(string url, bool js = false);
        Task<List<RssItem>> GetRssFeedAsync(string url);
    }
}