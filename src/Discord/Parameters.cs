namespace NW.Discord.Parameters
{
    public struct DiscordDeathQueryParameters
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

    public struct DiscordAnnouncementQueryParameters
    {
        public bool? important;
        public long fromtimestamp;
        public long totimestamp;
    }

    public struct DiscordLoginQueryParameters
    {
        public int? playerrole;
        public int? type;
        public long fromtimestamp;
        public long totimestamp;
        public int fromx;
        public int fromy;
        public int tox;
        public int toy;
        public string player;
        public string playeraccount;
    }

    public struct DiscordMessageQueryParameters
    {
        public int? senderrole;
        public int? type;
        public long fromtimestamp;
        public long totimestamp;
        public int fromx;
        public int fromy;
        public int tox;
        public int toy;
        public string sender;
        public string senderaccount;
    }
}