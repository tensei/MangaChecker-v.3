using MangaChecker.ViewModels.ViewModels;

namespace MangaChecker.ViewModels.Interfaces
{
    public interface IViewModelFactory
    {
        MangaListViewModel CreateMangaListViewModel { get; }
        AddMangaViewModel CreateAddMangaViewModel { get; }
        SettingsViewModel CreateSettingsViewModel { get; }
        ThemeViewModel CreateThemeViewModel { get; }
        NewMangaViewModel CreateNewMangaViewModel { get; }
        HistoryViewModel CreateHistoryViewModel { get; }
        GalleryViewModel CreateGalleryViewModel { get; }
    }
}