using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.Utilities;
using MangaChecker.ViewModels.Interfaces;

namespace MangaCheckerV3.Common {
    public class LinkParser : ILinkParser {
        private readonly IProviderSet _providerSet;
        public LinkParser(IProviderSet providerSet) {
            _providerSet = providerSet;
            //GetProviderFirstOrDefault(p => p.LinkIsMatch("test"));
        }

        public IProvider GetProviderFirstOrDefault(string link) => _providerSet.GetFirstOrDefault(p => p.LinkIsMatch(link));
        public IProvider GetProviderFirstOrDefault(Func<IProvider, bool> condition) => _providerSet.GetFirstOrDefault(condition);
    }
}
