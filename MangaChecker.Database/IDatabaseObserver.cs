using System.Collections.Generic;
using MangaChecker.Database.Tables;

namespace MangaChecker.Database {
    public interface IDatabaseObserver {
        void MangaEvent(Manga manga, DatabaseEvent _event);
        void GetMangaEvent(List<Manga> mangas, DatabaseEvent _event);
        void SettingEvent(Settings setting, DatabaseEvent _event);
        void SaveSettingEvent(List<Settings> setting, DatabaseEvent _event);
        void GetSettingEvent(List<Settings> settings, DatabaseEvent _event);
        void DatabaseEvent(DatabaseEvent _event);
    }
}