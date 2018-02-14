using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.Providers.Sites
{
    public class Mangahere : ProviderBase
    {
        public Mangahere()
        {
            DbName = nameof(Mangahere);
            LinktoSite = "http://mangahere.co/";
        }
    }
}