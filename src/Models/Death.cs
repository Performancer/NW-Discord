using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public class Death
    {
        Death()
        {
            TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public long TimeStamp { get; }

        public Character Killer { get; set; }

        [Required(ErrorMessage = "Killed is required")]
        public Character Killed { get; set; }

        [Required(ErrorMessage = "Weapon is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Weapon name has to be between 1-255 letters")]
        public string Weapon { get; set; }

        public bool FriendlyFire { get; set; }
    }
}