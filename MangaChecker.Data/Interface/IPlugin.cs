using System;

namespace MangaChecker.Data.Interface {
    public interface IPlugin : IDisposable {
        object SettingsView();
        void Initialize();
    }
}