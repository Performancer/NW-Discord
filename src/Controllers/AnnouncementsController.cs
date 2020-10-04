using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;
using NW.FilterAttributes;
using NW.Discord;

namespace NW.Controllers
{
    [Route("api/[controller]")]
    public class AnnouncementsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IDiscord _discord;

        public AnnouncementsController(IRepository repository, IDiscord discord)
        {
            _repository = repository;
            _discord = discord;
        }

        [HttpGet]
        public Task<Announcement[]> GetAll(
            [FromQuery(Name = "important")] bool? important,
            [FromQuery(Name = "from")] long fromTimestamp = long.MinValue,
            [FromQuery(Name = "to")] long toTimestamp = long.MaxValue
        ) {
            return _repository.GetAnnouncements(important, fromTimestamp, toTimestamp);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<Announcement> Post([FromBody] Announcement announcement)
        {
            return _discord.Notice(await _repository.AddAnnouncement(announcement));
        }
    }
}
