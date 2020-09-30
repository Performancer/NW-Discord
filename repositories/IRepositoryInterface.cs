using System;
using System.Threading.Tasks;
using NW.Models;

namespace NW.Repository
{
    public interface IRepositoryInterface
    {
        Task<Death> AddDeath(Death death);
        Task<Death[]> GetDeaths();

        Task<ChatMessage> AddChatMessage(ChatMessage message);
        Task<ChatMessage[]> GetChatMessages();

        Task<Announcement> AddAnnouncement(Announcement announcement);
        Task<Announcement[]> GetAnnouncements();
    }
}