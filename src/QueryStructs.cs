namespace NW.Query
{
    public struct DeathQuery
    {
        public int? KillerRole;
        public int? KilledRole;
        public bool? FriendlyFire;
        public int? MinScore;
        public int? MaxScore;
        public int? FromX;
        public int? FromY;
        public int? ToX;
        public int? ToY;
        public long? FromTimestamp;
        public long? ToTimestamp;
        public string Killer;
        public string KillerAccount;
        public string Killed;
        public string KilledAccount;
        public string Weapon;
    }

    public struct AnnouncementQuery
    {
        public bool? Important;
        public long? FromTimestamp;
        public long? ToTimestamp;
    }

    public struct LoginQuery
    {
        public int? PlayerRole;
        public int? Type;
        public long? FromTimestamp;
        public long? ToTimestamp;
        public int? FromX;
        public int? FromY;
        public int? ToX;
        public int? ToY;
        public string Player;
        public string PlayerAccount;
    }

    public struct MessageQuery
    {
        public int? SenderRole;
        public int? Type;
        public long? FromTimestamp;
        public long? ToTimestamp;
        public int? FromX;
        public int? FromY;
        public int? ToX;
        public int? ToY;
        public string Sender;
        public string SenderAccount;
    }
}