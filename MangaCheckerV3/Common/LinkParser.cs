using System;
using MangaChecker.Providers.Interfaces;
using MangaChecker.ViewModels.Interfaces;

namespace MangaCheckerV3.Common {
    public class LinkParser : ILinkParser {
        private readonly IProviderSet _providerSet;

        public LinkParser(IProviderSet providerSet) {
            _providerSet = providerSet;
            //GetProviderFirstOrDefault(p => p.LinkIsMatch("test"));
        }

        public IProvider GetProviderFirstOrDefault(string link) {
            return _providerSet.GetFirstOrDefault(p => p.LinkIsMatch(link));
        }

        public IProvider GetProviderFirstOrDefault(Func<IProvider, bool> condition) {
            return _providerSet.GetFirstOrDefault(condition);
        }
    }
}