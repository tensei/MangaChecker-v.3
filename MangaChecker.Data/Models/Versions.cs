//using SQLite;

using MangaChecker.Data.Interfaces;

namespace MangaChecker.Data.Models
{
    public class Versions : IVersions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}