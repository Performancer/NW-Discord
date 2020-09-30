using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;

namespace NW.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IRepository _repository;

        public AnnouncementsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<ChatMessage[]> GetAll()
        {
            return _repository.GetChatMessages();
        }

        [HttpPost]
        public Task<ChatMessage[]> Post([FromBody]ChatMessage message)
        {
            return _repository.AddChatMessage(message);
        }
    }
}
