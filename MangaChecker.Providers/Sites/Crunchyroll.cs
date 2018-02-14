using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Crunchyroll : ProviderBase
    {
        //http://utils.senpai.moe/
        public Crunchyroll()
        {
            DbName = nameof(Crunchyroll);
            LinktoSite = "http://www.crunchyroll.com/comics/manga";
        }
    }
}