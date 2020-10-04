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
    public class MessagesController : Controller
    {
        private readonly IRepository _repository;
        private readonly IDiscord _discord;

        public MessagesController(IRepository repository, IDiscord discord)
        {
            _repository = repository;
            _discord = discord;
        }

        [HttpGet]
        public Task<ChatMessage[]> GetAll(
            [FromQuery(Name = "sender-role")] int? senderRole,
            [FromQuery(Name = "type")] int? type,
            [FromQuery(Name = "fromTime")] long fromTimestamp = long.MinValue,
            [FromQuery(Name = "toTime")] long toTimestamp = long.MaxValue,
            [FromQuery(Name = "fromX")] int fromX = int.MinValue,
            [FromQuery(Name = "fromY")] int fromY = int.MinValue,
            [FromQuery(Name = "toX")] int toX = int.MaxValue,
            [FromQuery(Name = "toY")] int toY = int.MaxValue,
            [FromQuery(Name = "sender")] string sender = "",
            [FromQuery(Name = "sender-account")] string senderAccount = ""
        ) {
            return _repository.GetChatMessages(
                senderRole,
                fromX,
                fromY,
                toX,
                toY,
                type,
                fromTimestamp,
                toTimestamp,
                sender,
                senderAccount
            );
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ChatMessage> Post([FromBody] ChatMessage message)
        {
            return _discord.Notice(await _repository.AddChatMessage(message));
        }
    }
}
