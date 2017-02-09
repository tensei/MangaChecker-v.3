namespace MangaChecker.Data.Interface {
    public interface IPluginMetadata {
        string Title { get; }

        string Description { get; }

        string Version { get; }

        string Author { get; }
    }
}