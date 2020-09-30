using System;

public enum AccessRole
{
    Admin, Moderator, Standard
}

public enum MessageStyle
{
    Shout, Normal, Whisper
}

public class Vector3
{
    Vector3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}