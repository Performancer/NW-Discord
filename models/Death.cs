using System;

public class Death
{
    public Character Killer { get; set; }
    public Character Killed { get; set; }
    public string Weapon { get; set; }
    public DateTime TimeStamp { get; set; }
    public bool FriendOnFriend { get; set; }
}