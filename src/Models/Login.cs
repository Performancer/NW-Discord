using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public enum LoginType
    {
        Login,
        Logout
    }

    public class Login
    {
        public Login()
        {
            Id = Guid.NewGuid();
            TimeStamp = (long)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public Guid Id { get; set; }
        public long TimeStamp { get; set; }

        [Required(ErrorMessage = "LoginType is required")]
        [EnumDataType(typeof(LoginType))]
        public LoginType Type { get; set; }

        [Required(ErrorMessage = "Player is required")]
        public Character Player { get; set; }
    }
}