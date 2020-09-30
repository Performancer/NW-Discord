using System.Threading.Tasks;
using NW.Models;

namespace NW.Repository
{
    public class MongoDBRepository : IRepositoryInterface
    {
        public Task<Announcement> AddAnnouncement(Announcement announcement)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChatMessage> AddChatMessage(ChatMessage message)
        {
            throw new System.NotImplementedException();
        }

        public Task<Death> AddDeath(Death death)
        {
            throw new System.NotImplementedException();
        }

        public Task<Announcement[]> GetAnnouncements()
        {
            throw new System.NotImplementedException();
        }

        public Task<ChatMessage[]> GetChatMessages()
        {
            throw new System.NotImplementedException();
        }

        public Task<Death[]> GetDeaths()
        {
            throw new System.NotImplementedException();
        }
    }
}