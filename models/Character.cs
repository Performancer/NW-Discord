using System;

public enum AccessRole
{
    Admin, Moderator, Standard
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

public class Character
{
    public string AccountName { get; set; }
    public string CharacterName { get; set; }
    public AccessRole Role { get; set; }
    public Vector3 Location { get; set; }
    public int Score { get; set; }
}