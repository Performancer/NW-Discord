using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NW.Models
{
    public enum MessageType
    {
        Normal, Whisper, Shout
    }

    public class ChatMessage
    {
        public ChatMessage()
        {
            TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public long TimeStamp { get; }

        [Required(ErrorMessage = "Sender is required")]
        public Character Sender { get; set; }

        [Required(ErrorMessage = "Listeners are required")]
        public List<Character> Listeners { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Message has to be between 1-255 letters")]
        public string Message { get; set; }

        [EnumDataType(typeof(MessageType))]
        public MessageType Type { get; set; }
    }
}
