using MangaChecker.Data.Models;

namespace MangaChecker.Data.Interfaces {
    public interface IWindowFactory {
        void CreateViewerWindow(IManga manga);
        void CreateEditWindow(Manga manga);
    }
}