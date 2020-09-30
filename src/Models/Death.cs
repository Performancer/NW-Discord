using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public class Death
    {
        Death()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; }

        public Character Killer { get; set; }

        [Required(ErrorMessage = "Killed is required")]
        public Character Killed { get; set; }

        [Required(ErrorMessage = "Weapon is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Weapon name has to be between 1-255 letters")]
        public string Weapon { get; set; }

        public bool FriendlyFire { get; set; }
    }
}