using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;

using NW.Models;
using NW.Query;

namespace NW.Repository
{
    public class MongoDBRepository : IRepository
    {
        private readonly IMongoCollection<ChatMessage> _chatMessageCollection;
        private readonly IMongoCollection<Death> _deathCollection;
        private readonly IMongoCollection<Announcement> _announcementCollection;
        private readonly IMongoCollection<Login> _loginCollection;

        public MongoDBRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("NWDB");
            _chatMessageCollection = database.GetCollection<ChatMessage>("ChatMessages");
            _deathCollection = database.GetCollection<Death>("Deaths");
            _announcementCollection = database.GetCollection<Announcement>("Announcements");
            _loginCollection = database.GetCollection<Login>("Logins");
        }
        public async Task<Announcement> AddAnnouncement(Announcement announcement)
        {
            await _announcementCollection.InsertOneAsync(announcement);
            return announcement;
        }

        public async Task<Announcement[]> GetAnnouncements(AnnouncementQuery query, int? limit = null)
        {
            FilterDefinition<Announcement> filter = Builders<Announcement>.Filter.Empty;

            if(query.FromTimestamp != null)
                filter &= Builders<Announcement>.Filter.Gte(a => a.TimeStamp, query.FromTimestamp);

            if(query.ToTimestamp != null)
                filter &= Builders<Announcement>.Filter.Lte(a => a.TimeStamp, query.ToTimestamp);

            if (query.Important != null)
                filter &= Builders<Announcement>.Filter.Eq(a => a.Important, query.Important);

            var find = _announcementCollection.Find(filter);

            if(limit != null)
                find = find.Limit(limit);

            List<Announcement> announcements = await find.ToListAsync();

            return announcements.ToArray();
        }

        public async Task<ChatMessage> AddChatMessage(ChatMessage message)
        {
            await _chatMessageCollection.InsertOneAsync(message);
            return message;
        }

        public async Task<ChatMessage[]> GetChatMessages(MessageQuery query, int? limit = null)
        {
            FilterDefinition<ChatMessage> filter = Builders<ChatMessage>.Filter.Empty;

            if(query.FromTimestamp != null)
                filter &= Builders<ChatMessage>.Filter.Gte(m => m.TimeStamp, query.FromTimestamp);

            if(query.ToTimestamp != null)
                filter &= Builders<ChatMessage>.Filter.Lte(m => m.TimeStamp, query.ToTimestamp);

            if(query.FromX != null)
                filter &= Builders<ChatMessage>.Filter.Gte(m => m.Sender.Location.X, query.FromX);

            if(query.FromY != null)
                filter &= Builders<ChatMessage>.Filter.Gte(m => m.Sender.Location.Y, query.FromY);

            if(query.ToX != null)
                filter &= Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.X, query.ToX);

            if(query.ToY != null)
                filter &= Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.Y, query.ToY);

            if (query.SenderRole != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.Role, (AccessRole)query.SenderRole);

            if (query.Type != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Type, (MessageType)query.Type);

            if (query.Sender != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.Name, query.Sender);

            if (query.SenderAccount != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.AccountName, query.SenderAccount);

            var find = _chatMessageCollection.Find(filter);

            if(limit != null)
                find = find.Limit(limit);

            List<ChatMessage> chatMessages = await find.ToListAsync();

            return chatMessages.ToArray();
        }

        public async Task<Death> AddDeath(Death death)
        {
            await _deathCollection.InsertOneAsync(death);
            return death;
        }

        public async Task<Death[]> GetDeaths(DeathQuery query, int? limit = null)
        {
            FilterDefinition<Death> filter = Builders<Death>.Filter.Empty;
            
            if(query.FromTimestamp != null)
                filter &= Builders<Death>.Filter.Gte(d => d.TimeStamp, query.FromTimestamp);

            if(query.ToTimestamp != null)
                filter &= Builders<Death>.Filter.Lte(d => d.TimeStamp, query.ToTimestamp);

            if(query.MinScore != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Score, query.MinScore);

            if(query.MaxScore != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Score, query.MaxScore);

            if(query.FromX != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Location.X, query.FromX);

            if(query.FromY != null)
                filter &= Builders<Death>.Filter.Gte(d => d.Killed.Location.Y, query.FromY);

            if(query.ToX != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Location.X, query.ToX);

            if (query.ToY != null)
                filter &= Builders<Death>.Filter.Lte(d => d.Killed.Location.Y, query.ToY);

            if (query.KillerRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.Role, (AccessRole)query.KillerRole);

            if (query.KilledRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.Role, (AccessRole)query.KilledRole);

            if (query.FriendlyFire != null)
                filter &= Builders<Death>.Filter.Eq(d => d.FriendlyFire, query.FriendlyFire);

            if (query.Killer != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.Name, query.Killer);

            if (query.KillerAccount != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.AccountName, query.KillerAccount);

            if (query.Weapon != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Weapon, query.Weapon);

            if (query.Killed != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.Name, query.Killed);

            if (query.KilledAccount != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.AccountName, query.KilledAccount);

            var find = _deathCollection.Find(filter);

            if(limit != null)
                find = find.Limit(limit);

            List<Death> deaths = await find.ToListAsync();

            return deaths.ToArray();
        }

        public async Task<Login> AddLogin(Login login)
        {
            await _loginCollection.InsertOneAsync(login);
            return login;
        }

        public async Task<Login[]> GetLogins(LoginQuery query, int? limit = null)
        {
            FilterDefinition<Login> filter = Builders<Login>.Filter.Empty;

            if (query.FromTimestamp != null)
                filter &= Builders<Login>.Filter.Gte(x => x.TimeStamp, query.FromTimestamp);

            if (query.ToTimestamp != null)
                filter &= Builders<Login>.Filter.Lte(x => x.TimeStamp, query.ToTimestamp);

            if (query.FromX != null)
                filter &= Builders<Login>.Filter.Gte(x => x.Player.Location.X, query.FromX);

            if (query.FromY != null)
                filter &= Builders<Login>.Filter.Gte(x => x.Player.Location.Y, query.FromY);

            if (query.ToX != null)
                filter &= Builders<Login>.Filter.Lte(x => x.Player.Location.X, query.ToX);

            if (query.ToY != null)
                filter &= Builders<Login>.Filter.Lte(x => x.Player.Location.Y, query.ToY);

            if (query.PlayerRole != null)
                filter &= Builders<Login>.Filter.Gte(x => x.Player.Role, (AccessRole)query.PlayerRole);

            if (query.Type != null)
                filter &= Builders<Login>.Filter.Eq(x => x.Type, (LoginType)query.Type);

            if (query.Player != null)
                filter &= Builders<Login>.Filter.Eq(x => x.Player.Name, query.Player);

            if (query.PlayerAccount != null)
                filter &= Builders<Login>.Filter.Eq(x => x.Player.AccountName, query.PlayerAccount);

            var find = _loginCollection.Find(filter);

            if(limit != null)
                find = find.Limit(limit);

            List<Login> logins = await find.ToListAsync();

            return logins.ToArray();
        }
    }
}
