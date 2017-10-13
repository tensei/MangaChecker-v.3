namespace MangaChecker.Data.Interfaces
{
    public interface IVersions
    {
        int Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
    }
}