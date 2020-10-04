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
            var config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient(config);

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

        private void SetVariableIfFoundInQuery<T>(out T variable, string[] varNameToValuePair)
        {
            variable = default(T);

            if (nameof(variable) == varNameToValuePair[0])
            {
                variable = ConvertTo<T>(varNameToValuePair[1]);
            }
        }

        private async Task ParseQueryCommand(SocketMessage message)
        {
            string str = message.Content.Replace("!query ", "").ToLower();
            string[] args = str.Split(" ");
            Console.WriteLine("args len: " + args.Length + "\nCommands: " + str + "\n");

            int? killerrole = null;
            int? killedrole = null;
            bool? friendlyfire = null;
            int minscore = int.MinValue;
            int maxscore = int.MaxValue;
            int fromx = int.MinValue;
            int fromy = int.MinValue;
            int tox = int.MaxValue;
            int toy = int.MaxValue;
            long fromtimestamp = long.MinValue;
            long totimestamp = long.MaxValue;
            string killer = "";
            string killeraccount = "";
            string killed = "";
            string killedaccount = "";
            string weapon = "";




            switch (args[0])
            {
                case "deaths":
                    string[] vars = args[1].Split("&");

                    foreach (string var in vars)
                    {
                        string[] varNameToValuePair = var.Split("=");
                        Console.WriteLine("var: " + varNameToValuePair[0] + "\nvalue: " + varNameToValuePair[1] + "\n");

                        if (nameof(killerrole) == varNameToValuePair[0])
                            killerrole = int.Parse(varNameToValuePair[1]);

                        if (nameof(killedrole) == varNameToValuePair[0])
                            killerrole = int.Parse(varNameToValuePair[1]);

                        if (nameof(friendlyfire) == varNameToValuePair[0])
                            friendlyfire = bool.Parse(varNameToValuePair[1]);

                        if (nameof(minscore) == varNameToValuePair[0])
                            minscore = int.Parse(varNameToValuePair[1]);

                        if (nameof(maxscore) == varNameToValuePair[0])
                            maxscore = int.Parse(varNameToValuePair[1]);

                        if (nameof(fromx) == varNameToValuePair[0])
                            fromx = int.Parse(varNameToValuePair[1]);

                        if (nameof(fromy) == varNameToValuePair[0])
                            fromy = int.Parse(varNameToValuePair[1]);

                        if (nameof(tox) == varNameToValuePair[0])
                            tox = int.Parse(varNameToValuePair[1]);

                        if (nameof(toy) == varNameToValuePair[0])
                            toy = int.Parse(varNameToValuePair[1]);

                        if (nameof(fromtimestamp) == varNameToValuePair[0])
                            fromtimestamp = int.Parse(varNameToValuePair[1]);

                        if (nameof(totimestamp) == varNameToValuePair[0])
                            totimestamp = int.Parse(varNameToValuePair[1]);

                        if (nameof(killer) == varNameToValuePair[0])
                            killer = varNameToValuePair[1];

                        if (nameof(killed) == varNameToValuePair[0])
                            killed = varNameToValuePair[1];

                        if (nameof(killeraccount) == varNameToValuePair[0])
                            killeraccount = varNameToValuePair[1];

                        if (nameof(killedaccount) == varNameToValuePair[0])
                            killedaccount = varNameToValuePair[1];

                        if (nameof(weapon) == varNameToValuePair[0])
                            weapon = varNameToValuePair[1];
                    }
                    Console.WriteLine("Variables: {");
                    Console.WriteLine("\tkillerrole:" + killerrole);
                    Console.WriteLine("\tkilledrole:" + killedrole);
                    Console.WriteLine("\tfriendlyfire:" + friendlyfire);
                    Console.WriteLine("\tminscore:" + minscore);
                    Console.WriteLine("\tmaxscore:" + maxscore);
                    Console.WriteLine("\tfromx:" + fromx);
                    Console.WriteLine("\tfromy:" + fromy);
                    Console.WriteLine("\ttox:" + tox);
                    Console.WriteLine("\ttoy:" + toy);
                    Console.WriteLine("\tfromtimestamp:" + fromtimestamp);
                    Console.WriteLine("\ttotimestamp:" + totimestamp);
                    Console.WriteLine("\tkiller:" + killer);
                    Console.WriteLine("\tkilleraccount:" + killeraccount);
                    Console.WriteLine("\tkilled:" + killed);
                    Console.WriteLine("\tkilledaccount:" + killedaccount);
                    Console.WriteLine("\tweapon:" + weapon);
                    Console.WriteLine("}");

                    Death[] deaths = await _repository.GetDeaths(
                        killerrole,
                        killedrole,
                        minscore,
                        maxscore,
                        fromx,
                        fromy,
                        tox,
                        toy,
                        friendlyfire,
                        fromtimestamp,
                        totimestamp,
                        killer,
                        killeraccount,
                        weapon,
                        killed,
                        killedaccount
                    );

                    string deathFormatted = DeathsToString(deaths);
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

        private async Task<Death[]> GetDeaths(string args)
        {
            Death[] deaths = await _repository.GetDeaths(null, null, 0, int.MaxValue, int.MinValue, int.MinValue, int.MaxValue, int.MaxValue, null, 0, long.MaxValue, "", "", "", "", "");
            return deaths.OrderByDescending<Death, long>(d => d.TimeStamp).ToArray();
        }

        private string DeathsToString(Death[] deaths)
        {
            string msg = "Last 10 Deaths: \n";

            for (int i = 0; i < deaths.Length; i++)
                msg += "[" + deaths[i].TimeStamp + "] " + deaths[i].Killer.Name + " killed " + deaths[i].Killed.Name + " with " + deaths[i].Weapon + ".\n";

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
