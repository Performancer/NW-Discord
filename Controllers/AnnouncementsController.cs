using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;

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
        public Task<Announcement[]> GetAll()
        {
            return _repository.GetDeaths();
        }

        [HttpPost]
        public Task<Announcement[]> Post([FromBody]Announcement announcement)
        {
            return _repository.AddDeath(death);
        }
    }
}
