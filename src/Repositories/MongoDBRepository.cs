using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using MongoDB.Driver;
using MongoDB.Bson;


using NW.Models;

namespace NW.Repository
{
    public class MongoDBRepository : IRepository
    {
        private readonly IMongoCollection<ChatMessage> _ChatMessageCollection;
        private readonly IMongoCollection<Death> _DeathCollection;
        private readonly IMongoCollection<Announcement> _AnnouncementCollection;

        public MongoDBRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("NWDB");
            _ChatMessageCollection = database.GetCollection<ChatMessage>("ChatMessages");
            _DeathCollection = database.GetCollection<Death>("Deaths");
            _AnnouncementCollection = database.GetCollection<Announcement>("Announcements");
        }
        public async Task<Announcement> AddAnnouncement(Announcement announcement)
        {
            await _AnnouncementCollection.InsertOneAsync(announcement);
            return announcement;
        }

        public async Task<ChatMessage> AddChatMessage(ChatMessage message)
        {
            await _ChatMessageCollection.InsertOneAsync(message);
            return message;
        }

        public async Task<Death> AddDeath(Death death)
        {
            await _DeathCollection.InsertOneAsync(death);
            return death;
        }

        public async Task<Announcement[]> GetAnnouncements(
            bool? important,
            long fromTimestamp = 0,
            long toTimestamp = long.MaxValue)
        {
            FilterDefinition<Announcement> filter = Builders<Announcement>.Filter.And(
                Builders<Announcement>.Filter.Gte(a => a.TimeStamp, fromTimestamp),
                Builders<Announcement>.Filter.Lte(a => a.TimeStamp, toTimestamp)
            );

            if (important != null)
                filter &= Builders<Announcement>.Filter.Eq(a => a.Important, important);

            List<Announcement> announcements = await _AnnouncementCollection.Find(filter).ToListAsync();

            Console.WriteLine("AnnouncementCount=" + announcements.Count);
            return announcements.ToArray();
        }

        public async Task<ChatMessage[]> GetChatMessages(
            int? senderRole,
            int? fromX,
            int? fromY,
            int? toX,
            int? toY,
            int? type,
            long fromTimestamp = long.MinValue,
            long toTimestamp = long.MaxValue,
            string sender = "",
            string senderAccount = ""
        )
        {
            FilterDefinition<ChatMessage> filter = Builders<ChatMessage>.Filter.And(
                Builders<ChatMessage>.Filter.Gte(m => m.TimeStamp, fromTimestamp),
                Builders<ChatMessage>.Filter.Lte(m => m.TimeStamp, toTimestamp)
            );

            if (senderRole != null)
                filter &= Builders<ChatMessage>.Filter.Gte(m => m.Sender.Role, (AccessRole)senderRole);

            if (fromX != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Type, (MessageType)type);

            if (sender != "")
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.Name, sender);

            if (senderAccount != "")
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.AccountName, sender);


            List<ChatMessage> chatMessages = await _ChatMessageCollection.Find(filter).ToListAsync();

            Console.WriteLine("AnnouncementCount=" + chatMessages.Count);
            return chatMessages.ToArray();
        }

        public async Task<Death[]> GetDeaths(
            int? killerRole,
            int? killedRole,
            int? minScore,
            int? maxScore,
            int? fromX,
            int? fromY,
            int? toX,
            int? toY,
            bool? friendlyFire,
            long fromTimestamp = long.MinValue,
            long toTimestamp = long.MaxValue,
            string killer = "",
            string killerAccount = "",
            string weapon = "",
            string killed = "",
            string killedAccount = ""
        )
        {
            FilterDefinition<Death> filter1 = Builders<Death>.Filter.Empty;
            FilterDefinition<Death> filter = Builders<Death>.Filter.And(
                Builders<Death>.Filter.Gte(d => d.TimeStamp, fromTimestamp),
                Builders<Death>.Filter.Lte(d => d.TimeStamp, toTimestamp)
            );

            if (killerRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.Role, (AccessRole)killerRole);

            if (killedRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.Role, (AccessRole)killedRole);

            if (minScore != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Score, minScore);

            if (maxScore != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Score, maxScore);

            if (fromX != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Location.X, fromX);

            if (fromY != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Location.Y, fromY);

            if (toX != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Location.X, toX);

            if (toY != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Location.Y, toY);

            if (friendlyFire != null)
                filter &= Builders<Death>.Filter.Eq(d => d.FriendlyFire, friendlyFire);

            if (killer != "")
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.Name, killer);

            if (killerAccount != "")
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.AccountName, killerAccount);

            if (weapon != "")
                filter &= Builders<Death>.Filter.Eq(d => d.Weapon, weapon);

            if (killed != "")
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.Name, killed);

            if (killedAccount != "")
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.AccountName, killedAccount);

            List<Death> deaths = await _DeathCollection.Find(filter).ToListAsync();

            return deaths.ToArray();
        }
    }
}