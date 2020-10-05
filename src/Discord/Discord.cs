using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Discord;
using Discord.WebSocket;

using System.Net.Http.Headers;

using NW.Models;
using NW.Repository;
using System.Reflection;

namespace NW.Discord
{
    public class DiscordClient : IDiscord
    {
        private DiscordSocketClient _client;

        private HttpClient _httpClient;
        private ChannelManager _channels;
        private IRepository _repository;

        public DiscordClient(IRepository repository)
        {
            _repository = repository;
        }

        public ChannelManager GetChannels()
        {
            _httpClient = new HttpClient();
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

        public async void Notice(ulong channelId, string message)
        {
            IMessageChannel channel;

            while ((channel = _client.GetChannel(channelId) as IMessageChannel) == null)
            {

                await Task.Delay(1000);
            }

            await channel.SendMessageAsync(message);
        }

        public Announcement Notice(Announcement announcement)
        {
            string message = announcement.Message;

            if (announcement.Important)
                message += " @everyone";

            Notice(_channels.AnnouncementChannelID, message);
            return announcement;
        }

        public ChatMessage Notice(ChatMessage chatMessage)
        {
            string message = chatMessage.Sender.ToString();

            switch (chatMessage.Type)
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

            if (death.Killer != null && death.Killer.IsPlayer() && death.Killed != null && death.Killed.IsPlayer())
            {
                message += "**PLAYER KILL:** ";
            }

            message += death.Killed + " was killed by ";

            if (death.Killer != null)
                message += death.Killer + " with " + death.Weapon + " as a weapon.";
            else
                message += death.Weapon;

            if (death.FriendlyFire)
                message += " This was friendly fire.";

            Notice(_channels.FeedChannelID, message);
            return death;
        }

        public Login Notice(Login login)
        {
            string message = "[" + login.Player.AccountName + "] " + login.Player;

            switch (login.Type)
            {
                case LoginType.Login: message += " has logged in."; break;
                case LoginType.Logout: message += " has logged out."; break;
            }

            Notice(_channels.FeedChannelID, message);
            return login;
        }


        public static T ConvertTo<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private void SetVariableIfFoundInQuery<T, Y>(ref Y @params, string fieldName, T fieldValue, string[] varNameToValuePair)
        {

            if (fieldName == varNameToValuePair[0])
            {
                var field = @params.GetType().GetField(fieldName);
                var newValue = ConvertTo<T>(varNameToValuePair[1]);

                TypedReference tr = __makeref(@params);
                field.SetValueDirect(tr, newValue);

            }
        }

        public struct DiscordDeathParameters
        {
            public int? killerrole;
            public int? killedrole;
            public bool? friendlyfire;
            public int minscore;
            public int maxscore;
            public int fromx;
            public int fromy;
            public int tox;
            public int toy;
            public long fromtimestamp;
            public long totimestamp;
            public string killer;
            public string killeraccount;
            public string killed;
            public string killedaccount;
            public string weapon;
        }
        private void ParseVariables<T>(string arguments, ref T @params)
        {
            Console.WriteLine("arguments:" + arguments);
            string[] vars = arguments.Split("&");

            foreach (string var in vars)
            {
                string[] varNameToValuePair = var.Split("=");
                Console.WriteLine("var: " + varNameToValuePair[0] + "\nvalue: " + varNameToValuePair[1] + "\n");

                foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    SetVariableIfFoundInQuery(ref @params, field.Name, field.GetValue(@params), varNameToValuePair);
                    Console.WriteLine("{0} : {1}", field.Name, field.GetValue(@params));
                }
            }

        }

        private async Task ParseQueryCommand(SocketMessage message)
        {
            string str = message.Content.Replace("!query ", "").ToLower();
            string[] args = str.Split(" ");
            Console.WriteLine("args len: " + args.Length + "\nCommands: " + str + "\n");

            var deathParams = new DiscordDeathParameters
            {
                killerrole = null,
                killedrole = null,
                friendlyfire = null,
                minscore = int.MinValue,
                maxscore = int.MaxValue,
                fromx = int.MinValue,
                fromy = int.MinValue,
                tox = int.MaxValue,
                toy = int.MaxValue,
                fromtimestamp = long.MinValue,
                totimestamp = long.MaxValue,
                killer = "",
                killeraccount = "",
                killed = "",
                killedaccount = "",
                weapon = ""
            };

            switch (args[0])
            {
                case "deaths":
                    if (args[1] != "")
                    {
                        Console.WriteLine("Something to parse");
                        ParseVariables(args[1], ref deathParams);
                    }

                    /*
                    Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                        deathParams.killerrole,
                        deathParams.killedrole,
                        deathParams.minscore,
                        deathParams.maxscore,
                        deathParams.fromx,
                        deathParams.fromy,
                        deathParams.tox,
                        deathParams.toy,
                        deathParams.friendlyfire,
                        deathParams.fromtimestamp,
                        deathParams.totimestamp,
                        deathParams.killer,
                        deathParams.killeraccount,
                        deathParams.weapon,
                        deathParams.killed,
                        deathParams.killedaccount
                    );
                    */
                    Death[] deaths = await _repository.GetDeaths(
                        deathParams.killerrole,
                        deathParams.killedrole,
                        deathParams.minscore,
                        deathParams.maxscore,
                        deathParams.fromx,
                        deathParams.fromy,
                        deathParams.tox,
                        deathParams.toy,
                        deathParams.friendlyfire,
                        deathParams.fromtimestamp,
                        deathParams.totimestamp,
                        deathParams.killer,
                        deathParams.killeraccount,
                        deathParams.weapon,
                        deathParams.killed,
                        deathParams.killedaccount
                    );

                    int count = 10;

                    if (args.Length >= 3)
                    {
                        int success = 0;
                        if (int.TryParse(args[2], out success))
                        {
                            count = int.Parse(args[2]);
                        }
                    }


                    string deathFormatted = DeathsToString(deaths.ToList().TakeLast(count).ToArray());
                    await message.Channel.SendMessageAsync(deathFormatted);

                    break;

                case "logins":
                    break;

                case "announcements":
                    break;

                case "messages":
                    break;
            }
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private string DeathsToString(Death[] deaths)
        {
            Console.WriteLine("Deaths: " + deaths.Length);
            string msg = "Last " + deaths.Length + " Deaths: \n";

            for (int i = deaths.Length - 1; i >= 0; --i)
                msg += (deaths.Length - i) + "\t[" + UnixTimeStampToDateTime(deaths[i].TimeStamp) + "] " + deaths[i].Killer.Name + " killed " + deaths[i].Killed.Name + " with " + deaths[i].Weapon + ".\n";

            return msg;
        }

        private string MessagesToString(ChatMessage[] messages)
        {
            Console.WriteLine("Messages: " + messages.Length);
            string msg = "Last " + messages.Length + " Messages: \n";

            for (int i = messages.Length - 1; i >= 0; --i)
                msg += (messages.Length - i) + "\t[" + UnixTimeStampToDateTime(messages[i].TimeStamp) + "] Type:[" + messages[i].Type.ToString() + "]" + messages[i].Sender.Name + ": " + messages[i].Message + " with " + ".\n";

            return msg;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            SocketGuildChannel channel = message.Channel as SocketGuildChannel;
            var guild = channel.Guild;

            Console.WriteLine("> [" + guild.Name + "] : [" + channel.Name + "] : [" + message.Author.Username + "] : " + message.Content);

            if (message.Channel.Id == _channels.QueryChannelID)
            {
                if (message.Content.StartsWith("!query"))
                {
                    await ParseQueryCommand(message);
                }
            }
        }
    }
}
