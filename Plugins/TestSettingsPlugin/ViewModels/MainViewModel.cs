using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using SQLite;
using TestSettingsPlugin.New_Tables;
using TestSettingsPlugin.Old_Tables;

namespace TestSettingsPlugin.ViewModels {
    public class MainViewModel : ViewModelBase {
        private string _output;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
            Start = new ActionCommand(StartMigrationAsync);
        }

        public ICommand Start { get; }
        public ICommand Stop { get; }
        public ICommand Pause { get; }

        public string Output {
            get { return _output; }
            set {
                _output = value;
                OnPropertyChanged();
            }
        }

        private async void StartMigrationAsync() {
            var conn = new SQLiteAsyncConnection("MangaDB.sqlite");

            var db = new LiteDatabase("mcv3.db");
            var newdb = db.GetCollection<Manga>("Manga");

            var batoto = await conn.Table<batoto>().ToListAsync();
            WriteOutput($"batoto {batoto.Count} entries");

            foreach (var batoto1 in batoto) {
                WriteOutput(
                    $"Inserting {batoto1.name} | {batoto1.chapter} | {batoto1.last_update} | {batoto1.link} | {batoto1.rss_url}");
                newdb.Insert(new Manga {
                    Name = batoto1.name,
                    Chapter = float.Parse(batoto1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(batoto1.last_update),
                    Link = batoto1.link,
                    Rss = batoto1.rss_url,
                    Site = "Batoto"
                });
            }

            var backlog = await conn.Table<backlog>().ToListAsync();
            WriteOutput($"backlog {backlog.Count} entries");
            foreach (var backlog1 in backlog) {
                WriteOutput(
                    $"Inserting {backlog1.name} | {backlog1.chapter} | {backlog1.last_update} | {backlog1.link} | {backlog1.rss_url}");
                var date = DateTime.Parse(backlog1.last_update);
                newdb.Insert(new Manga {
                    Name = backlog1.name,
                    Chapter = float.Parse(backlog1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = date,
                    Link = backlog1.link,
                    Rss = backlog1.rss_url,
                    Site = "Backlog"
                });
            }

            var goscan = await conn.Table<goscanlation>().ToListAsync();
            WriteOutput($"GameOfScanlation {goscan.Count} entries");
            foreach (var GameOfScanlation1 in goscan) {
                WriteOutput(
                    $"Inserting {GameOfScanlation1.name} | {GameOfScanlation1.chapter} | {GameOfScanlation1.last_update} | {GameOfScanlation1.link} | {GameOfScanlation1.rss_url}");
                newdb.Insert(new Manga {
                    Name = GameOfScanlation1.name,
                    Chapter = float.Parse(GameOfScanlation1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(GameOfScanlation1.last_update),
                    Link = GameOfScanlation1.link,
                    Rss = GameOfScanlation1.rss_url,
                    Site = "GameOfScanlation"
                });
            }

            var heymang = await conn.Table<heymanga>().ToListAsync();
            WriteOutput($"HeyManga {heymang.Count} entries");
            foreach (var HeyManga1 in heymang) {
                WriteOutput(
                    $"Inserting {HeyManga1.name} | {HeyManga1.chapter} | {HeyManga1.last_update} | {HeyManga1.link} | {HeyManga1.rss_url}");
                newdb.Insert(new Manga {
                    Name = HeyManga1.name,
                    Chapter = float.Parse(HeyManga1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(HeyManga1.last_update),
                    Link = HeyManga1.link,
                    Rss = HeyManga1.rss_url,
                    Site = "HeyManga"
                });
            }

            var jaim = await conn.Table<jaiminisbox>().ToListAsync();
            WriteOutput($"Jaiminisbox {jaim.Count} entries");
            foreach (var Jaiminisbox1 in jaim) {
                WriteOutput(
                    $"Inserting {Jaiminisbox1.name} | {Jaiminisbox1.chapter} | {Jaiminisbox1.last_update} | {Jaiminisbox1.link} | {Jaiminisbox1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Jaiminisbox1.name,
                    Chapter = float.Parse(Jaiminisbox1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Jaiminisbox1.last_update),
                    Link = Jaiminisbox1.link,
                    Rss = Jaiminisbox1.rss_url,
                    Site = "Jaiminisbox"
                });
            }

            var kirei = await conn.Table<kireicake>().ToListAsync();
            WriteOutput($"KireiCake {kirei.Count} entries");
            foreach (var KireiCake1 in kirei) {
                WriteOutput(
                    $"Inserting {KireiCake1.name} | {KireiCake1.chapter} | {KireiCake1.last_update} | {KireiCake1.link} | {KireiCake1.rss_url}");
                newdb.Insert(new Manga {
                    Name = KireiCake1.name,
                    Chapter = float.Parse(KireiCake1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(KireiCake1.last_update),
                    Link = KireiCake1.link,
                    Rss = KireiCake1.rss_url,
                    Site = "KireiCake"
                });
            }

            var kiss = await conn.Table<kissmanga>().ToListAsync();
            WriteOutput($"Kissmanga {kiss.Count} entries");
            foreach (var Kissmanga1 in kiss) {
                WriteOutput(
                    $"Inserting {Kissmanga1.name} | {Kissmanga1.chapter} | {Kissmanga1.last_update} | {Kissmanga1.link} | {Kissmanga1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Kissmanga1.name,
                    Chapter = float.Parse(Kissmanga1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Kissmanga1.last_update),
                    Link = Kissmanga1.link,
                    Rss = Kissmanga1.rss_url,
                    Site = "Kissmanga"
                });
            }

            var mfox = await conn.Table<mangafox>().ToListAsync();
            WriteOutput($"Mangafox {mfox.Count} entries");
            foreach (var Mangafox1 in mfox) {
                WriteOutput(
                    $"Inserting {Mangafox1.name} | {Mangafox1.chapter} | {Mangafox1.last_update} | {Mangafox1.link} | {Mangafox1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Mangafox1.name,
                    Chapter = float.Parse(Mangafox1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Mangafox1.last_update),
                    Link = Mangafox1.link,
                    Rss = Mangafox1.rss_url,
                    Site = "Mangafox"
                });
            }

            var mhere = await conn.Table<mangahere>().ToListAsync();
            WriteOutput($"Mangahere {mhere.Count} entries");
            foreach (var Mangahere1 in mhere) {
                WriteOutput(
                    $"Inserting {Mangahere1.name} | {Mangahere1.chapter} | {Mangahere1.last_update} | {Mangahere1.link} | {Mangahere1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Mangahere1.name,
                    Chapter = float.Parse(Mangahere1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Mangahere1.last_update),
                    Link = Mangahere1.link,
                    Rss = Mangahere1.rss_url,
                    Site = "Mangahere"
                });
            }

            var mreader = await conn.Table<mangareader>().ToListAsync();
            WriteOutput($"Mangareader {mreader.Count} entries");
            foreach (var Mangareader1 in mreader) {
                WriteOutput(
                    $"Inserting {Mangareader1.name} | {Mangareader1.chapter} | {Mangareader1.last_update} | {Mangareader1.link} | {Mangareader1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Mangareader1.name,
                    Chapter = float.Parse(Mangareader1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Mangareader1.last_update),
                    Link = Mangareader1.link,
                    Rss = Mangareader1.rss_url,
                    Site = "Mangareader"
                });
            }

            var mstream = await conn.Table<mangastream>().ToListAsync();
            WriteOutput($"Mangastream {mstream.Count} entries");
            foreach (var Mangastream1 in mstream) {
                WriteOutput(
                    $"Inserting {Mangastream1.name} | {Mangastream1.chapter} | {Mangastream1.last_update} | {Mangastream1.link} | {Mangastream1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Mangastream1.name,
                    Chapter = float.Parse(Mangastream1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Mangastream1.last_update),
                    Link = Mangastream1.link,
                    Rss = Mangastream1.rss_url,
                    Site = "Mangastream"
                });
            }

            var yo = await conn.Table<yomanga>().ToListAsync();
            WriteOutput($"YoManga {yo.Count} entries");
            foreach (var YoManga1 in yo) {
                WriteOutput(
                    $"Inserting {YoManga1.name} | {YoManga1.chapter} | {YoManga1.last_update} | {YoManga1.link} | {YoManga1.rss_url}");
                newdb.Insert(new Manga {
                    Name = YoManga1.name,
                    Chapter = float.Parse(YoManga1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(YoManga1.last_update),
                    Link = YoManga1.link,
                    Rss = YoManga1.rss_url,
                    Site = "YoManga"
                });
            }

            var web = await conn.Table<webtoons>().ToListAsync();
            WriteOutput($"Webtoons {web.Count} entries");
            foreach (var Webtoons1 in web) {
                WriteOutput(
                    $"Inserting {Webtoons1.name} | {Webtoons1.chapter} | {Webtoons1.last_update} | {Webtoons1.link} | {Webtoons1.rss_url}");
                newdb.Insert(new Manga {
                    Name = Webtoons1.name,
                    Chapter = float.Parse(Webtoons1.chapter, CultureInfo.InvariantCulture),
                    Added = DateTime.Now,
                    Updated = DateTime.Parse(Webtoons1.last_update),
                    Link = Webtoons1.link,
                    Rss = Webtoons1.rss_url,
                    Site = "Webtoons"
                });
            }
        }

        private void WriteOutput(string text) {
            Output += $"[{DateTime.Now}] {text}\n";
        }
    }

    public class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}