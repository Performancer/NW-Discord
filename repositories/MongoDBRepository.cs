using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Announcement[]> GetAnnouncements()
        {
            List<Announcement> announcements = await _AnnouncementCollection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("AnnouncementCount=" + announcements.Count);
            return announcements.ToArray();
        }

        public async Task<ChatMessage[]> GetChatMessages()
        {
            List<ChatMessage> chatMessages = await _ChatMessageCollection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("AnnouncementCount=" + chatMessages.Count);
            return chatMessages.ToArray();
        }

        public async Task<Death[]> GetDeaths()
        {
            List<Death> deaths = await _DeathCollection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("AnnouncementCount=" + deaths.Count);
            return deaths.ToArray();
        }
    }
}