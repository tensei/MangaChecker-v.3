using System;
using System.Collections.Generic;

namespace MangaChecker.Providers.Interfaces
{
    public interface IProviderSet
    {
        List<string> GetDbNames { get; }
        List<IProvider> GetAll { get; }
        IProvider GetFirstOrDefault(Func<IProvider, bool> condition);
    }
}