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
            FilterDefinition<Announcement> filter =
                Builders<Announcement>.Filter.Gte(a => a.TimeStamp, fromTimestamp)
                & Builders<Announcement>.Filter.Lte(a => a.TimeStamp, toTimestamp)
                & Builders<Announcement>.Filter.Eq(a => a.Important, important);

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
            FilterDefinition<ChatMessage> filter =
                Builders<ChatMessage>.Filter.Eq(m => m.Sender.Role, (AccessRole)senderRole)
                & Builders<ChatMessage>.Filter.Gte(m => m.Sender.Location.X, fromX)
                & Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.X, toX)
                & Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.Y, fromY)
                & Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.Y, toY)
                & Builders<ChatMessage>.Filter.Eq(m => m.Type, (MessageType)type)
                & Builders<ChatMessage>.Filter.Gte(m => m.TimeStamp, fromTimestamp)
                & Builders<ChatMessage>.Filter.Lte(m => m.TimeStamp, toTimestamp)
                & Builders<ChatMessage>.Filter.Eq(m => m.Sender.Name, sender)
                & Builders<ChatMessage>.Filter.Eq(m => m.Sender.AccountName, sender);

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
            FilterDefinition<Death> filter =
                Builders<Death>.Filter.Eq(d => d.Killer.Role, (AccessRole)killerRole)
                & Builders<Death>.Filter.Eq(d => d.Killed.Role, (AccessRole)killedRole)
                & Builders<Death>.Filter.Gte(d => d.Killed.Score, minScore)
                & Builders<Death>.Filter.Lte(d => d.Killed.Score, maxScore)
                & Builders<Death>.Filter.Gte(d => d.Killed.Location.X, fromX)
                & Builders<Death>.Filter.Lte(d => d.Killed.Location.X, toX)
                & Builders<Death>.Filter.Gte(d => d.Killed.Location.Y, fromY)
                & Builders<Death>.Filter.Lte(d => d.Killed.Location.Y, toY)
                & Builders<Death>.Filter.Eq(d => d.FriendlyFire, friendlyFire)
                & Builders<Death>.Filter.Gte(d => d.TimeStamp, fromTimestamp)
                & Builders<Death>.Filter.Lte(d => d.TimeStamp, toTimestamp)
                & Builders<Death>.Filter.Eq(d => d.Killer.Name, killer)
                & Builders<Death>.Filter.Eq(d => d.Killer.AccountName, killerAccount)
                & Builders<Death>.Filter.Eq(d => d.Weapon, weapon)
                & Builders<Death>.Filter.Eq(d => d.Killed.Name, killed)
                & Builders<Death>.Filter.Eq(d => d.Killed.AccountName, killedAccount);

            List<Death> deaths = await _DeathCollection.Find(filter).ToListAsync();
            Console.WriteLine("AnnouncementCount=" + deaths.Count);
            return deaths.ToArray();
        }
    }
}