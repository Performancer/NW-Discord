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
            [FromQuery(Name = "fromTime")] long fromTimestamp = long.MinValue,
            [FromQuery(Name = "toTime")] long toTimestamp = long.MaxValue,
            [FromQuery(Name = "fromX")] int fromX = int.MinValue,
            [FromQuery(Name = "fromY")] int fromY = int.MinValue,
            [FromQuery(Name = "toX")] int toX = int.MaxValue,
            [FromQuery(Name = "toY")] int toY = int.MaxValue,
            [FromQuery(Name = "player")] string player = "",
            [FromQuery(Name = "player-account")] string playerAccount = ""
        ) {
            return _repository.GetLogins(
                playerRole,
                fromX,
                fromY,
                toX,
                toY,
                type,
                fromTimestamp,
                toTimestamp,
                player,
                playerAccount
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
