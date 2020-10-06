using System;
using System.Threading.Tasks;
using NW.Models;
using NW.Query;

namespace NW.Repository
{
    public interface IRepository
    {
        Task<Death> AddDeath(Death death);
        Task<Death[]> GetDeaths(DeathQuery query, int? limit = null);

        Task<ChatMessage> AddChatMessage(ChatMessage message);
        Task<ChatMessage[]> GetChatMessages(MessageQuery query, int? limit = null);

        Task<Login> AddLogin(Login login);
        Task<Login[]> GetLogins(LoginQuery query, int? limit = null);

        Task<Announcement> AddAnnouncement(Announcement announcement);
        Task<Announcement[]> GetAnnouncements(AnnouncementQuery query, int? limit = null);
    }
}