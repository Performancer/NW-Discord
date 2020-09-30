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
            [FromQuery(Name = "fromTime")] int fromTimestamp,
            [FromQuery(Name = "toTime")] int toTimestamp,
            [FromQuery(Name = "sender")] string killer,
            [FromQuery(Name = "sender-account")] string killerAccount,
            [FromQuery(Name = "sender-role")] int? killerRole,
            [FromQuery(Name = "fromX")] int? fromX,
            [FromQuery(Name = "fromY")] int? fromY,
            [FromQuery(Name = "toX")] int? toX,
            [FromQuery(Name = "toY")] int? toY,
            [FromQuery(Name = "type")] int? type
        ) {
            return _repository.GetChatMessages();
        }

        [HttpPost]
        [ValidateModel]
        public Task<ChatMessage> Post([FromBody]ChatMessage message)
        {
            return _repository.AddChatMessage(message);
        }
    }
}
