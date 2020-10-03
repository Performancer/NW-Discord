using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;
using NW.FilterAttributes;

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
            [FromQuery(Name = "fromTime")] long fromTimestamp,
            [FromQuery(Name = "toTime")] long toTimestamp,
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
        )
        {
            return _repository.GetDeaths(
                killerRole,
                killedRole,
                minScore,
                maxScore,
                fromX,
                fromY,
                toX,
                toY,
                friendlyFire,
                fromTimestamp,
                toTimestamp,
                killer,
                killerAccount,
                weapon,
                killed,
                killedAccount
            );
        }

        [HttpPost]
        [ValidateModel]
        public Task<Death> Post([FromBody] Death death)
        {
            return _repository.AddDeath(death);
        }
    }
}
