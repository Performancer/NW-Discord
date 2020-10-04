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

        public async void Login()
        {
            _client = new DiscordSocketClient();

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            string token = File.ReadAllText("discord-key.txt");

            Console.WriteLine("Discord: Logging in...");
            await _client.LoginAsync(TokenType.Bot, token);
            Console.WriteLine("Discord: Starting...");
            await _client.StartAsync();
            Console.WriteLine("Discord: Ready!");
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

        public Announcement Notice(Announcement announcement)
        {
            string message = announcement.Message;

            if(announcement.Important)
                message += " @everyone";

            Notice(message);
            return announcement;
        }

        public ChatMessage Notice(ChatMessage chatMessage)
        {
            string message = chatMessage.Sender.ToString();

            switch(chatMessage.Type)
            {
                case Models.MessageType.Normal: message += " says"; break;
                case Models.MessageType.Whisper: message += " whispers"; break;
                case Models.MessageType.Shout: message += " shouts"; break;
            }

            message += ": " + chatMessage.Message;

            Notice(message);
            return chatMessage;
        }

        public Death Notice(Death death)
        {
            string message = "";

            if(death.Killer != null && death.Killer.IsPlayer() && death.Killed != null && death.Killed.IsPlayer())
            {
                message += "**PLAYER KILL:** ";
            }

            message += death.Killed + " was killed by ";

            if(death.Killer != null)
                message += death.Killer + " with " + death.Weapon + " as a weapon.";
            else
                message += death.Weapon;

            if(death.FriendlyFire)
                message += " This was friendly fire.";

            Notice(message);
            return death;
        }
    }
}
