using MangaCheckerV3.ViewModels;

namespace MangaCheckerV3.Interfaces {
    public interface IViewModelFactory {
        MangaListViewModel CreateMangaListViewModel { get; }
        AddMangaViewModel CreateAddMangaViewModel { get; }
        SettingsViewModel CreateSettingsViewModel { get; }
        PluginsViewModel CreatePluginsViewModel { get; }
        ThemeViewModel CreateThemeViewModel { get; }
        NewMangaViewModel CreateNewMangaViewModel { get; }
        HistoryViewModel CreateHistoryViewModel { get; }
    }
}