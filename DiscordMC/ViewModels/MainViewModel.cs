using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Discord;
using MangaChecker.Data.Enums;
using MangaChecker.Data.Models;
using MangaCheckerV3.Common;
using Settings = DiscordMC.Properties.Settings;

namespace DiscordMC.ViewModels {
    public class MainViewModel {
        public MainViewModel() {
            GlobalVariables.LiteDb.MangaEvent += LiteDbOnMangaEvent;

            ConnectCommand = new ActionCommand(Start);
            DisconnectCommand = new ActionCommand(async () => await CloseAsync());
            IsAutoConnect = Settings.Default.AutoConnect;
            Token = Settings.Default.Token;
            ServerId = Settings.Default.ServerId;
            ChannelId = Settings.Default.ChannelId;
            if (IsAutoConnect) {
                Start();
            }
        }

        public bool IsAutoConnect { get; set; }

        public ICommand ConnectCommand { get; }

        public ICommand DisconnectCommand { get; }

        public string Token { get; set; }

        public ulong ServerId { get; set; }

        public ulong ChannelId { get; set; }

        public async Task CloseAsync() {
            await _client.Disconnect();
            _client?.Dispose();
            Settings.Default.AutoConnect = IsAutoConnect;
            Settings.Default.Token = Token;
            Settings.Default.ServerId = ServerId;
            Settings.Default.ChannelId = ChannelId;
            Settings.Default.Save();
            _client = null;
        }
        private void LiteDbOnMangaEvent(object sender, MangaEnum mangaEnum) {

            if (mangaEnum != MangaEnum.Update) {
                return;
            }
            var m = (Manga)sender;
            try {
                var server = _client?.GetServer(ServerId);
                var ch = server?.GetChannel(ChannelId);
                ch?.SendMessage($"New Release!!\n{m.Name} {m.Newest},\n{m.Link}");
            } catch {
                // hmm
            }
        }

        private DiscordClient _client;

        private void Start() {
            if (_client != null || ServerId == 0 || ChannelId == 0 || string.IsNullOrWhiteSpace(Token)) {
                return;
            }

            Task.Run(() => {
                _client = new DiscordClient();
                _client.ExecuteAndWait(async () => {
                    await _client.Connect(Token, TokenType.Bot);
                });
                _client.SetGame("Looking for new Memes");
            });
        }
    }
}
