using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;

namespace NW.Controllers
{
    [Route("api/[controller]")]
    public class DeathsController : Controller
    {
        private readonly IRepository _repository;

        public DeathsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<Death[]> GetAll(
            [FromQuery(Name = "fromTime")] int fromTimestamp,
            [FromQuery(Name = "toTime")] int toTimestamp,
            [FromQuery(Name = "killer")] string killer,
            [FromQuery(Name = "killer-account")] string killerAccount,
            [FromQuery(Name = "killer-role")] int? killerRole,
            [FromQuery(Name = "killed")] string killed,
            [FromQuery(Name = "killed-account")] string killedAccount,
            [FromQuery(Name = "killed-role")] int? killedRole,
            [FromQuery(Name = "minscore")] int? minScore,
            [FromQuery(Name = "maxscore")] int? maxScore,
            [FromQuery(Name = "fromX")] int? fromX,
            [FromQuery(Name = "fromY")] int? fromY,
            [FromQuery(Name = "toX")] int? toX,
            [FromQuery(Name = "toY")] int? toY,
            [FromQuery(Name = "weapon")] string weapon,
            [FromQuery(Name = "friendly")] bool? friendlyFire
        ){
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            DateTime fromDateTime = epoch.AddSeconds(fromTimestamp);
            DateTime toDateTime = epoch.AddSeconds(toTimestamp);

            return _repository.GetDeaths();
        }

        [HttpPost]
        public Task<Death> Post([FromBody]Death death)
        {
            return _repository.AddDeath(death);
        }
    }
}
