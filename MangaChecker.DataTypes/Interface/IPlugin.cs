using System;

namespace MangaChecker.DataTypes.Interface {
    public interface IPlugin : IDisposable {
        object SettingsView();
        void Initialize();
    }
}