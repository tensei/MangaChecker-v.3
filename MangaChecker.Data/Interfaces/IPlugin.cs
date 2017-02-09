using System;

namespace MangaChecker.Data.Interfaces {
    public interface IPlugin : IDisposable {
        object SettingsView();
        void Initialize();
    }
}