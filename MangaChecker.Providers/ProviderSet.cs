using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.Providers {
    public class ProviderSet : IProviderSet {
        private readonly IEnumerable<IProvider> _providers;
        public ProviderSet(IEnumerable<IProvider> providers) {
            _providers = providers;
        }

        public List<string> GetDbNames => _providers.ToList().Select(p => p.DbName).ToList();
        public List<IProvider> GetAll => _providers.ToList();
        public IProvider GetFirstOrDefault(Func<IProvider, bool> condition) => _providers.FirstOrDefault(condition);
    }
}
