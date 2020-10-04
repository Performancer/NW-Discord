
using NW.Models;

namespace NW.Discord
{
    public interface IDiscord
    {
        void Notice(ulong channelId, string message);
        Announcement Notice(Announcement announcement);
        ChatMessage Notice(ChatMessage chatMessage);
        Death Notice(Death death);
    }
}