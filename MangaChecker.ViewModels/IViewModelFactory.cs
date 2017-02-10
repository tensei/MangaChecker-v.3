using MangaChecker.ViewModels.ViewModels;

namespace MangaChecker.ViewModels {
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