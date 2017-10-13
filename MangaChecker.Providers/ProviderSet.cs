using System;
using System.Collections.Generic;
using System.Linq;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.Providers
{
    public class ProviderSet : IProviderSet
    {
        private readonly IEnumerable<IProvider> _providers;

        public ProviderSet(IEnumerable<IProvider> providers)
        {
            _providers = providers;
        }

        public List<string> GetDbNames => _providers.ToList().Select(p => p.DbName).ToList();
        public List<IProvider> GetAll => _providers.ToList();

        public IProvider GetFirstOrDefault(Func<IProvider, bool> condition)
        {
            return _providers.FirstOrDefault(condition);
        }
    }
}