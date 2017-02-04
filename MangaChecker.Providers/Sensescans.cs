using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MangaChecker.Database.Tables;
using MangaChecker.DataTypes.Interface;

namespace MangaChecker.Providers {
    public class Sensescans : ISite {

        public async Task CheckAll() {
            throw new NotImplementedException();
        }

        public async Task<object> CheckOne(object manga) {
            throw new NotImplementedException();
        }

        public async Task<Tuple<List<object>, int>> GetImagesTaskAsync(string url) {
            throw new NotImplementedException();
        }

        public async Task<object> FindMangaInfoOnSite(string url) {
            throw new NotImplementedException();
        }

        public Regex LinkRegex() {
            return new Regex("");
        }

        public string DbName => "Sensescans";

        public bool ViewEnabled => false;
        public string LinktoSite => "http://sensescans.com/reader/";
    }
}
