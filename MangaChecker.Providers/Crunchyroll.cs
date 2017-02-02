using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;

namespace MangaChecker.Providers {
    public class Crunchyroll : ISite {
        //http://utils.senpai.moe/

        public async Task CheckAll() {
            throw new NotImplementedException();
        }

        public async Task<Manga> CheckOne(Manga manga) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<Manga> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public string DbSettingName() {
            return "Crunchyroll";
        }

        public Regex LinkRegex() {
            return new Regex("");
        }
    }
}