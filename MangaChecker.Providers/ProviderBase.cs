using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.Providers
{
    public class ProviderBase : IProvider
    {
        public string DbName { get; set; }
        public bool ViewEnabled { get; set; }
        public string LinktoSite { get; set; }


        public virtual bool LinkIsMatch(string link)
        {
            var regex = new Regex("");
            return regex.IsMatch(link);
        }
        public virtual Task CheckAll(Action<IManga> status)
        {
            throw new NotImplementedException();
        }

        public virtual Task<object> CheckOne(object manga)
        {
            throw new NotImplementedException();
        }

        public virtual Task<(List<object>, int)> GetImagesTaskAsync(string url)
        {
            throw new NotImplementedException();
        }

        public virtual Task<object> FindMangaInfoOnSite(string url)
        {
            throw new NotImplementedException();
        }
    }
}
