using MangaChecker.Data.Models;

namespace MangaChecker.Data.Interfaces
{
    public interface IWindowFactory
    {
        void CreateViewerWindow(IManga manga);
        void CreateViewerWindow(IManga manga, object provider);
        void CreateEditWindow(Manga manga);
    }
}