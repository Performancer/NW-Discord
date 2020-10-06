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
            [FromQuery(Name = "from-time")] long? fromTimestamp,
            [FromQuery(Name = "to-time")] long? toTimestamp,
            [FromQuery(Name = "from-x")] int? fromX,
            [FromQuery(Name = "from-y")] int? fromY,
            [FromQuery(Name = "to-x")] int? toX,
            [FromQuery(Name = "to-y")] int? toY,
            [FromQuery(Name = "sender")] string sender,
            [FromQuery(Name = "sender-account")] string senderAccount
        ) {
            return _repository.GetChatMessages(
                new MessageQuery
                {
                    SenderRole = senderRole,
                    FromX = fromX,
                    FromY = fromY,
                    ToX = toX,
                    ToY = toY,
                    Type = type,
                    FromTimestamp = fromTimestamp,
                    ToTimestamp = toTimestamp,
                    Sender = sender,
                    SenderAccount = senderAccount
                }
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
