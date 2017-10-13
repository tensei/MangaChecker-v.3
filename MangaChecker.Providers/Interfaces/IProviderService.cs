﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaChecker.Providers.Interfaces
{
    public interface IProviderService
    {
        bool Pause { get; set; }
        List<IProvider> Providers { get; }
        string Status { get; set; }
        bool Stop { get; set; }
        int Timer { get; set; }

        bool Add(IProvider site);
        void Close();
        bool Remove(IProvider site);
        Task Run();
    }
}