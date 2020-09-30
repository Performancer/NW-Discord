using System;
using System.Collections.Generic;

public enum MessageStyle
{
    Shout, Normal, Whisper
}

public class ChatMessage
{
    public Character Sender { get; set; }
    public List<Character> Listeners { get; set; }
    public AccessRole SenderRole { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
    public Vector3 Location { get; set; }
    public MessageStyle Style { get; set; }
}