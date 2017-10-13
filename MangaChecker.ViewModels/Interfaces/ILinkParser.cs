using System;
using MangaChecker.Providers.Interfaces;

namespace MangaChecker.ViewModels.Interfaces
{
    public interface ILinkParser
    {
        IProvider GetProviderFirstOrDefault(string link);
        IProvider GetProviderFirstOrDefault(Func<IProvider, bool> condition);
    }
}