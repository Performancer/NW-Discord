using System;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NW.Models;
using NW.Repository;

namespace NW.Discord
{
    public class DiscordClient : IDiscord
    {
        private DiscordSocketClient _client;
        private ChannelManager _channels;
        private IRepository _repository;

        public DiscordClient(IRepository repository)
        {
            _repository = repository;
        }

        public ChannelManager GetChannels()
        {
            var file = "discord-channels.txt";

            if (!File.Exists(file))
                throw new FileNotFoundException("discord-channels.txt was not found");

            var channels = JsonConvert.DeserializeObject<ChannelManager>(File.ReadAllText(file));
            
            Console.WriteLine("Announcement Channel: " + channels.AnnouncementChannelID);
            Console.WriteLine("Feed Channel: " + channels.FeedChannelID);
            Console.WriteLine("Query Channel: " + channels.QueryChannelID);

            return channels;
        }

        public async void Login()
        {
            _channels = GetChannels();
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
    
        public async void Notice(ulong channelId, string message)
        {
            IMessageChannel channel;

            while((channel = _client.GetChannel(channelId) as IMessageChannel) == null) {
                await Task.Delay(1000);
            }

            await channel.SendMessageAsync(message);
        }

        public Announcement Notice(Announcement announcement)
        {
            string message = announcement.Message;

            if(announcement.Important)
                message += " @everyone";

            Notice(_channels.AnnouncementChannelID, message);
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

            Notice(_channels.FeedChannelID, message);
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

            Notice(_channels.FeedChannelID, message);
            return death;
        }

        public Login Notice(Login login)
        {
            string message = "[" + login.Player.AccountName + "] " + login.Player;

            switch(login.Type)
            {
                case LoginType.Login: message += " has logged in."; break;
                case LoginType.Logout: message += " has logged out."; break;
            }

            Notice(_channels.FeedChannelID, message);  
            return login;
        }
    }
}
