using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaChecker.Data.Interface {
    public interface IProviderService {
        List<IProvider> Providers { get; set; }
        bool Pause { get; set; }
        string Status { get; set; }
        bool Stop { get; set; }
        int Timer { get; set; }

        bool Add(IProvider site);
        bool Remove(IProvider site);
        Task Run();
    }
}