using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;
using NW.FilterAttributes;
using NW.Discord;
using NW.Query;

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
            [FromQuery(Name = "minscore")] int? minScore,
            [FromQuery(Name = "maxscore")] int? maxScore,
            [FromQuery(Name = "fromX")] int? fromX,
            [FromQuery(Name = "fromY")] int? fromY,
            [FromQuery(Name = "toX")] int? toX,
            [FromQuery(Name = "toY")] int? toY,
            [FromQuery(Name = "fromTime")] long? fromTimestamp,
            [FromQuery(Name = "toTime")] long? toTimestamp,
            [FromQuery(Name = "killer")] string killer,
            [FromQuery(Name = "killer-account")] string killerAccount,
            [FromQuery(Name = "killed")] string killed,
            [FromQuery(Name = "killed-account")] string killedAccount,
            [FromQuery(Name = "weapon")] string weapon
        ) {
            return _repository.GetDeaths( 
                new DeathQuery
                {
                    KillerRole = killerRole,
                    KilledRole = killedRole,
                    MinScore = minScore,
                    MaxScore = maxScore,
                    FromX = fromX,
                    FromY = fromY,
                    ToX = toX,
                    ToY = toY,
                    FriendlyFire = friendlyFire,
                    FromTimestamp = fromTimestamp,
                    ToTimestamp = toTimestamp,
                    Killer = killer,
                    KillerAccount = killerAccount,
                    Weapon = weapon,
                    Killed = killed,
                    KilledAccount = killedAccount
                }
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
