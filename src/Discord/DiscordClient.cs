using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NW.Models;
using NW.Repository;
using NW.Query;
using NW.Exceptions;

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
            _client = new DiscordSocketClient(new DiscordSocketConfig { MessageCacheSize = 100 });

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            string token = File.ReadAllText("discord-key.txt");

            Console.WriteLine("Discord: Logging in...");
            await _client.LoginAsync(TokenType.Bot, token);
            Console.WriteLine("Discord: Starting...");
            await _client.StartAsync();

            _client.MessageReceived += MessageReceived;

            _client.Ready += () =>
            {
                Console.WriteLine("Discord: Ready!");
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
  
        private string ListTimestampables(ITimestampable[] timestampables)
        {
            if (timestampables.Length == 0)
                return "0 items found.";

            string header = "Last " + timestampables.Length + " items: \n";

            string content = "";
            int i = 0;
            
            foreach(var item in timestampables.OrderByDescending(t => t.GetTimestamp()).ToList())
                content += (++i) + "\t[" + UnixTimeStampToDateTime(item.GetTimestamp()) + "] " + item + "\n";

            return header + "```css\n" + content + "```";
        }

        private const string COMMAND = "!q";

        private string GetCommandType(string message, out string query)
        {
            string[] arr = message.Split(' ');

            if(arr.Length == 0 || arr.Length > 2)
                throw new InvalidQueryException();

            if(arr.Length == 2)
                query = arr[1];
            else
                query = "";

            return arr[0];
        }

        private async Task<string> HandleQueryCommand(string message)
        {
            message = message.Trim();

            if(message.StartsWith(COMMAND))
            {
                message = message.Substring(COMMAND.Length).TrimStart();
                
                string type = GetCommandType(message, out string query);

                switch(type)
                {
                    case "deaths": 
                    {
                        var q = query != "" ? JsonConvert.DeserializeObject<DeathQuery>(query) : new DeathQuery();
                        Death[] deaths = await _repository.GetDeaths(q);
                        return ListTimestampables(deaths);
                    }
                    case "announcements": 
                    {
                        var q = query != "" ? JsonConvert.DeserializeObject<AnnouncementQuery>(query) : new AnnouncementQuery();
                        Announcement[] announcements = await _repository.GetAnnouncements(q);

                        return ListTimestampables(announcements);
                    }
                    case "messages": 
                    {
                        var q = query != "" ? JsonConvert.DeserializeObject<MessageQuery>(query) : new MessageQuery();
                        ChatMessage[] messages = await _repository.GetChatMessages(q);

                        return ListTimestampables(messages);
                    }
                    case "logins": 
                    {
                        var q = query != "" ? JsonConvert.DeserializeObject<LoginQuery>(query) : new LoginQuery();
                        Login[] logins = await _repository.GetLogins(q);

                        return ListTimestampables(logins);
                    }
                }
            }

            throw new InvalidQueryException();
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if(message.Author.IsBot)
                return;

            SocketGuildChannel channel = message.Channel as SocketGuildChannel;
            var guild = channel.Guild;

            Console.WriteLine("> [" + guild.Name + "] : [" + channel.Name + "] : [" + message.Author.Username + "] : " + message.Content);

            if (message.Channel.Id == _channels.QueryChannelID)
            {
                string response;
                
                try
                {
                    response = await HandleQueryCommand(message.Content);
                } 
                catch (InvalidQueryException e)
                {
                    response = "Usage: " + COMMAND + " [deaths|announcements|messages|logins] [options]";
                }
                catch (JsonReaderException e )
                {
                    response = "Error: Invalid Query JSON";
                }
                catch (JsonSerializationException e )
                {
                    response = "Error: Invalid Query JSON";
                }

                await message.Channel.SendMessageAsync(response);
            }
        }
    
        public async void Notice(ulong channelId, string message)
        {
            IMessageChannel channel;

            while((channel = _client.GetChannel(channelId) as IMessageChannel) == null)
                await Task.Delay(1000);

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
