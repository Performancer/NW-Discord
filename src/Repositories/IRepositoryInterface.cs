using System;
using System.Threading.Tasks;
using NW.Models;

namespace NW.Repository
{
    public interface IRepository
    {
        Task<Death> AddDeath(Death death);
        Task<Death[]> GetDeaths(
            int? killerRole,
            int? killedRole,
            int? minScore,
            int? maxScore,
            int? fromX,
            int? fromY,
            int? toX,
            int? toY,
            bool? friendlyFire,
            long fromTimestamp,
            long toTimestamp,
            string killer,
            string killerAccount,
            string weapon,
            string killed,
            string killedAccount
        );

        Task<ChatMessage> AddChatMessage(ChatMessage message);
        Task<ChatMessage[]> GetChatMessages(

            int? senderRole,
            int? fromX,
            int? fromY,
            int? toX,
            int? toY,
            int? type,
            long fromTimestamp,
            long toTimestamp,
            string sender,
            string senderAccount
        );

        Task<Announcement> AddAnnouncement(Announcement announcement);
        Task<Announcement[]> GetAnnouncements(bool? important, long fromTimestamp = 0, long toTimestamp = long.MaxValue);
    }
}