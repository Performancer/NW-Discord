using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;
using NW.FilterAttributes;

namespace NW.Controllers
{
    [Route("api/[controller]")]
    public class AnnouncementsController : Controller
    {
        private readonly IRepository _repository;

        public AnnouncementsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<Announcement[]> GetAll(
            [FromQuery(Name = "from")] long fromTimestamp,
            [FromQuery(Name = "to")] long toTimestamp,
            [FromQuery(Name = "important")] bool? important
        )
        {
            return _repository.GetAnnouncements(important, fromTimestamp, toTimestamp);
        }

        [HttpPost]
        [ValidateModel]
        public Task<Announcement> Post([FromBody] Announcement announcement)
        {
            return _repository.AddAnnouncement(announcement);
        }
    }
}
