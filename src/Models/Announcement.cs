using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public class Announcement
    {
        public Announcement()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Message has to be between 5-255 letters")]
        public string Message { get; set; }

        public bool Important { get; set; }
    }
}
