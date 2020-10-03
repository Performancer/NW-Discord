using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace NW.Discord
{
    public class DiscordClient : IDiscord
    {
        private DiscordSocketClient _client;
        private ulong _channelID = 559740041988014082;

        public async Task Login()
        {
            _client = new DiscordSocketClient();

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            string token = File.ReadAllText("discord-key.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public async void SendMessage(string message)
        {
            if (_client == null)
               Login();

            IMessageChannel channel;

            while((channel = _client.GetChannel(_channelID) as IMessageChannel) == null) {
                await Task.Delay(1000);
            }

            await channel.SendMessageAsync(message);
        }
    }
}
