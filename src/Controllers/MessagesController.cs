using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;
using NW.FilterAttributes;

namespace NW.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IRepository _repository;

        public MessagesController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<ChatMessage[]> GetAll(
            [FromQuery(Name = "fromTime")] long fromTimestamp,
            [FromQuery(Name = "toTime")] long toTimestamp,

            [FromQuery(Name = "sender-role")] int? senderRole,
            [FromQuery(Name = "fromX")] int? fromX,
            [FromQuery(Name = "fromY")] int? fromY,
            [FromQuery(Name = "toX")] int? toX,
            [FromQuery(Name = "toY")] int? toY,
            [FromQuery(Name = "type")] int? type,
            [FromQuery(Name = "sender")] string sender,
            [FromQuery(Name = "sender-account")] string senderAccount
        )
        {
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
        public Task<ChatMessage> Post([FromBody] ChatMessage message)
        {
            return _repository.AddChatMessage(message);
        }
    }
}
