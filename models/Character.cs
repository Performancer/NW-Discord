using System;

namespace NW.Models
{
    public struct Character
    {
        public string AccountName { get; set; }
        public string CharacterName { get; set; }
        public AccessRole Role { get; set; }
        public Vector3 Location { get; set; }
        public int Score { get; set; }
    }
}