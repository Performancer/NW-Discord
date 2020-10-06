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
    public class LoginsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IDiscord _discord;

        public LoginsController(IRepository repository, IDiscord discord)
        {
            _repository = repository;
            _discord = discord;
        }

        [HttpGet]
        public Task<Login[]> GetAll(
            [FromQuery(Name = "player-role")] int? playerRole,
            [FromQuery(Name = "type")] int? type,
            [FromQuery(Name = "fromTime")] long? fromTimestamp,
            [FromQuery(Name = "toTime")] long? toTimestamp,
            [FromQuery(Name = "fromX")] int? fromX,
            [FromQuery(Name = "fromY")] int? fromY,
            [FromQuery(Name = "toX")] int? toX,
            [FromQuery(Name = "toY")] int? toY,
            [FromQuery(Name = "player")] string player,
            [FromQuery(Name = "player-account")] string playerAccount
        ) {
            return _repository.GetLogins(
                new LoginQuery 
                {
                    PlayerRole = playerRole,
                    FromX = fromX,
                    FromY = fromY,
                    ToX = toX,
                    ToY = toY,
                    Type = type,
                    FromTimestamp = fromTimestamp,
                    ToTimestamp = toTimestamp,
                    Player = player,
                    PlayerAccount = playerAccount
                }
            );
        }

        [HttpPost]
        [ValidateModel]
        public async Task<Login> Post([FromBody] Login login)
        {
            return _discord.Notice(await _repository.AddLogin(login));
        }
    }
}
