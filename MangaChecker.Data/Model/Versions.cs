//using SQLite;

using MangaChecker.Data.Interface;

namespace MangaChecker.Data.Model {
    public class Versions : IVersions {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}