using System;

namespace MangaChecker.Data.Interfaces
{
    public interface ISettings
    {
        int Id { get; set; }
        string Setting { get; set; }
        string Link { get; set; }
        DateTime Created { get; set; }
        int Active { get; set; }
        bool OpenLinks { get; set; }
    }
}