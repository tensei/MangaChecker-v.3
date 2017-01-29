namespace MangaChecker.DataTypes.Interfaces {
    public interface IPluginMetadata {
        string Title { get; }

        string Description { get; }

        string Version { get; }

        string Author { get; }
    }
}