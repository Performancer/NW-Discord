using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NW.Models;

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
    
        public async void Notice(string message)
        {
            if (_client == null)
               Login();

            IMessageChannel channel;

            while((channel = _client.GetChannel(_channelID) as IMessageChannel) == null) {
                await Task.Delay(1000);
            }

            await channel.SendMessageAsync(message);
        }

        public Death Notice(Death death)
        {
            string message;

            if(death.Killer != null && death.Killer.IsPlayer() && death.Killed != null && death.Killed.IsPlayer())
            {
                message = "PLAYER KILL: ";
            }
            else
            {
                message = "Death: ";
            }

            message += death.Killed + " was killed by ";

            if(death.Killer != null)
                message += death.Killer + " with " + death.Weapon + " as a weapon.";
            else
                message += death.Weapon;

            if(death.FriendlyFire)
                message += " This was friendly fire.";

            Notice("**" + message + "**");
            return death;
        }
    }
}
