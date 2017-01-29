using System.Collections.Generic;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;

namespace MangaChecker.DataTypes.Interface {
    public interface ISite {
        Task CheckAll();
        Task<Manga> CheckOne(Manga manga);
        Task<IEnumerable<object>> GetImagesTaskAsync(string url);
        Task<Manga> FindMangaInfoOnSite(string url);
    }
}