using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;

using NW.Models;

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

        public async Task<Announcement[]> GetAnnouncements(
            bool? important,
            long fromTimestamp,
            long toTimestamp
        ) {
            FilterDefinition<Announcement> filter = Builders<Announcement>.Filter.And(
                Builders<Announcement>.Filter.Gte(a => a.TimeStamp, fromTimestamp),
                Builders<Announcement>.Filter.Lte(a => a.TimeStamp, toTimestamp)
            );

            if (important != null)
                filter &= Builders<Announcement>.Filter.Eq(a => a.Important, important);

            List<Announcement> announcements = await _announcementCollection.Find(filter).ToListAsync();

            return announcements.ToArray();
        }

        public async Task<ChatMessage> AddChatMessage(ChatMessage message)
        {
            await _chatMessageCollection.InsertOneAsync(message);
            return message;
        }

        public async Task<ChatMessage[]> GetChatMessages(
            int? senderRole,
            int fromX,
            int fromY,
            int toX,
            int toY,
            int? type,
            long fromTimestamp,
            long toTimestamp,
            string sender,
            string senderAccount
        ) {
            FilterDefinition<ChatMessage> filter = Builders<ChatMessage>.Filter.And(
                Builders<ChatMessage>.Filter.Gte(m => m.TimeStamp, fromTimestamp),
                Builders<ChatMessage>.Filter.Lte(m => m.TimeStamp, toTimestamp),
                Builders<ChatMessage>.Filter.Gte(m => m.Sender.Location.X, fromX),
                Builders<ChatMessage>.Filter.Gte(m => m.Sender.Location.Y, fromY),
                Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.X, toX),
                Builders<ChatMessage>.Filter.Lte(m => m.Sender.Location.Y, toY)
            );

            if (senderRole != null)
                filter &= Builders<ChatMessage>.Filter.Gte(m => m.Sender.Role, (AccessRole)senderRole);

            if (type != null)
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Type, (MessageType)type);

            if (sender != "")
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.Name, sender);

            if (senderAccount != "")
                filter &= Builders<ChatMessage>.Filter.Eq(m => m.Sender.AccountName, senderAccount);

            List<ChatMessage> chatMessages = await _chatMessageCollection.Find(filter).ToListAsync();

            return chatMessages.ToArray();
        }

        public async Task<Death> AddDeath(Death death)
        {
            await _deathCollection.InsertOneAsync(death);
            return death;
        }

        public async Task<Death[]> GetDeaths(
            int? killerRole,
            int? killedRole,
            int minScore,
            int maxScore,
            int fromX,
            int fromY,
            int toX,
            int toY,
            bool? friendlyFire,
            long fromTimestamp,
            long toTimestamp,
            string killer,
            string killerAccount,
            string weapon,
            string killed,
            string killedAccount
        ) {
            FilterDefinition<Death> filter = Builders<Death>.Filter.And(
                Builders<Death>.Filter.Gte(d => d.TimeStamp, fromTimestamp),
                Builders<Death>.Filter.Lte(d => d.TimeStamp, toTimestamp),
                Builders<Death>.Filter.Gte(d => d.Killed.Score, minScore),
                Builders<Death>.Filter.Lte(d => d.Killed.Score, maxScore),
                Builders<Death>.Filter.Gte(d => d.Killed.Location.X, fromX),
                Builders<Death>.Filter.Gte(d => d.Killed.Location.Y, fromY),
                Builders<Death>.Filter.Lte(d => d.Killed.Location.X, toX),
                Builders<Death>.Filter.Lte(d => d.Killed.Location.Y, toY)
            );

            if (killerRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killer.Role, (AccessRole)killerRole);

            if (killedRole != null)
                filter &= Builders<Death>.Filter.Eq(d => d.Killed.Role, (AccessRole)killedRole);

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

            List<Death> deaths = await _deathCollection.Find(filter).ToListAsync();

            return deaths.ToArray();
        }

        public async Task<Login> AddLogin(Login login)
        {
            await _loginCollection.InsertOneAsync(login);
            return login;
        }

        public async Task<Login[]> GetLogins(
            int? playerRole,
            int fromX,
            int fromY,
            int toX,
            int toY,
            int? type,
            long fromTimestamp,
            long toTimestamp,
            string player,
            string playerAccount
        ) {
            FilterDefinition<Login> filter = Builders<Login>.Filter.And(
                Builders<Login>.Filter.Gte(x => x.TimeStamp, fromTimestamp),
                Builders<Login>.Filter.Lte(x => x.TimeStamp, toTimestamp),
                Builders<Login>.Filter.Gte(x => x.Player.Location.X, fromX),
                Builders<Login>.Filter.Gte(x => x.Player.Location.Y, fromY),
                Builders<Login>.Filter.Lte(x => x.Player.Location.X, toX),
                Builders<Login>.Filter.Lte(x => x.Player.Location.Y, toY)
            );

            if (playerRole != null)
                filter &= Builders<Login>.Filter.Gte(x => x.Player.Role, (AccessRole)playerRole);

            if (type != null)
                filter &= Builders<Login>.Filter.Eq(x => x.Type, (LoginType)type);

            if (player != "")
                filter &= Builders<Login>.Filter.Eq(x => x.Player.Name, player);

            if (playerAccount != "")
                filter &= Builders<Login>.Filter.Eq(x => x.Player.AccountName, playerAccount);

            List<Login> logins = await _loginCollection.Find(filter).ToListAsync();

            return logins.ToArray();
        }
    }
}
