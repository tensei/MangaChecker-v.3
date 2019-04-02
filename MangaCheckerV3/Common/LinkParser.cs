using System;
using System.Collections.Generic;
using System.Linq;
using MangaChecker.Providers.Interfaces;
using MangaChecker.ViewModels.Interfaces;

namespace MangaCheckerV3.Common
{
    public class LinkParser : ILinkParser
    {
        private readonly List<IProvider> _providerSet;

        public LinkParser(IEnumerable<IProvider> providerSet)
        {
            _providerSet = providerSet.ToList();
            //GetProviderFirstOrDefault(p => p.LinkIsMatch("test"));
        }

        public IProvider GetProviderFirstOrDefault(string link)
        {
            return _providerSet.FirstOrDefault(p => p.LinkIsMatch(link));
        }

        public IProvider GetProviderFirstOrDefault(Func<IProvider, bool> condition)
        {
            return _providerSet.FirstOrDefault(condition);
        }
    }
}