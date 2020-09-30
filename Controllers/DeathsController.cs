using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NW.Models;
using NW.Repository;

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
        public Task<Death[]> GetAll()
        {
            return _repository.GetDeaths();
        }

        [HttpPost]
        public Task<Death> Post([FromBody]Death death)
        {
            return _repository.AddDeath(death);
        }
    }
}
