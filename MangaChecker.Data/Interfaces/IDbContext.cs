using System;
using System.Collections.Generic;
using System.Linq;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Models;

namespace MangaChecker.Data.Interfaces
{
    public interface IDbContext
    {
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
        void UpdateDatabase(Versions dbv, Dictionary<string, string> providers);
        void CreateDatabase(Dictionary<string, string> providers);
        string CheckDbVersion(Dictionary<string, string> providers);
        void Dispose();
    }
}