using System;
using System.Collections.Generic;
using System.Linq;
using MangaChecker.Data.Enum;
using MangaChecker.Data.Model;

namespace MangaChecker.Data.Interface {
    public interface ILiteDb {
        event EventHandler<MangaEnum> MangaEvent;
        event EventHandler<DatabaseEnum> DbEvent;
        event EventHandler<SettingEnum> SettingEvent;
        IOrderedEnumerable<Manga> GetHistory();
        IEnumerable<Manga> GetAllMangas();
        IOrderedEnumerable<Manga> GetAllNewMangas();
        IEnumerable<Manga> GetMangasFrom(string site);
        void InsertManga(Manga manga);
        void InsertHistory(Manga manga);
        void InsertNewManga(Manga manga);
        void DeleteNewManga(Manga manga);
        void Update(Manga manga, bool history = false);
        void UpdateMangaTrans(List<Manga> manga, bool history = false);
        void Delete(Manga manga);
        void DeleteHistory(Manga manga);
        List<Settings> GetAllSettings();
        Settings GetSettingsFor(string setting);
        void SaveSettings(List<Settings> settings);
        int GetRefreshTime();
        bool GetOpenLinks();
        void UpdateDatabase(Versions dbv, List<IProvider> providers);
        void CreateDatabase(List<IProvider> providers);
        string CheckDbVersion(List<IProvider> providers);
        void Dispose();
    }
}