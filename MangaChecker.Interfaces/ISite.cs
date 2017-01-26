using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;

namespace MangaChecker.Interfaces {
	public interface ISite {
        Task CheckAll();
	    Task<Manga> CheckOne(Manga manga);
	    Task<IEnumerable<object>> GetImagesTask(string url);
	}
}
