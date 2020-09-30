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
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; }

        [Required(ErrorMessage = "Sender is required")]
        public Character Sender { get; set; }
        
         [Required(ErrorMessage = "Listeners are required")]
        public List<Character> Listeners { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Message needs to be between 1-255 letters")]
        public string Message { get; set; }

        [EnumDataType(typeof(MessageType))]
        public MessageType Type { get; set; }
    }
}
