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
            [FromQuery(Name = "from")] int fromTimestamp,
            [FromQuery(Name = "to")] int toTimestamp,
            [FromQuery(Name = "important")] bool? important
        ) {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            DateTime fromDateTime = epoch.AddSeconds(fromTimestamp);
            DateTime toDateTime = epoch.AddSeconds(toTimestamp);

            return _repository.GetAnnouncements();
        }

        [HttpPost]
        [ValidateModel]
        public Task<Announcement> Post([FromBody]Announcement announcement)
        {
            return _repository.AddAnnouncement(announcement);
        }
    }
}
