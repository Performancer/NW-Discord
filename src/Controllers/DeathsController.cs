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
    public class DeathsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IDiscord _discord;

        public DeathsController(IRepository repository, IDiscord discord)
        {
            _repository = repository;
            _discord = discord;
        }

        [HttpGet]
        public Task<Death[]> GetAll(
            [FromQuery(Name = "killer-role")] int? killerRole,
            [FromQuery(Name = "killed-role")] int? killedRole,
            [FromQuery(Name = "friendly")] bool? friendlyFire,
            [FromQuery(Name = "minscore")] int minScore = int.MinValue,
            [FromQuery(Name = "maxscore")] int maxScore = int.MaxValue,
            [FromQuery(Name = "fromX")] int fromX = int.MinValue,
            [FromQuery(Name = "fromY")] int fromY = int.MinValue,
            [FromQuery(Name = "toX")] int toX = int.MaxValue,
            [FromQuery(Name = "toY")] int toY = int.MaxValue,
            [FromQuery(Name = "fromTime")] long fromTimestamp = long.MinValue,
            [FromQuery(Name = "toTime")] long toTimestamp = long.MaxValue,
            [FromQuery(Name = "killer")] string killer = "",
            [FromQuery(Name = "killer-account")] string killerAccount = "",
            [FromQuery(Name = "killed")] string killed = "",
            [FromQuery(Name = "killed-account")] string killedAccount = "",
            [FromQuery(Name = "weapon")] string weapon = ""
        ) {
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
        public async Task<Death> Post([FromBody] Death death)
        {
            return _discord.Notice(await _repository.AddDeath(death));
        }
    }
}
