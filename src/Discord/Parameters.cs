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
        public long fromTimestamp;
        public long toTimestamp;
    }

    public struct DiscordLoginQueryParameters
    {
        public int? playerRole;
        public int? type;
        public long fromTimestamp;
        public long toTimestamp;
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;
        public string player;
        public string playerAccount;
    }

    public struct DiscordMessageQueryParameters
    {
        public int? senderRole;
        public int? type;
        public long fromTimestamp;
        public long toTimestamp;
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;
        public string sender;
        public string senderAccount;
    }
}