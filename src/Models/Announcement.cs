using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public class Announcement
    {
        public Announcement()
        {
            TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public long TimeStamp { get; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Message has to be between 5-255 letters")]
        public string Message { get; set; }

        public bool Important { get; set; }
    }
}
