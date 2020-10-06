using System;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public class Announcement : ITimestampable
    {
        public Announcement()
        {
            TimeStamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Id = Guid.NewGuid();
        }
   
        public Guid Id { get; set; }

        public long TimeStamp { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Message has to be between 5-255 letters")]
        public string Message { get; set; }

        public bool Important { get; set; }

        public long GetTimestamp()
        {
            return TimeStamp;
        }

        public override string ToString()
        {
            return "[Important:" + Important + "] " + Message;
        }
    }
}
