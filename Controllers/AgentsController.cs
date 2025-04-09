using Microsoft.AspNetCore.Mvc;
using FloAPI.Data;
using FloAPI.Models;
using MongoDB.Driver;

namespace FloAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly MongoDbContext _db;

        public AgentsController(MongoDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = _db.Agents.Find(_ => true).ToList();
            return Ok(clients);
        }

        [HttpPost]
        public IActionResult Create(Agent agent)
        {
            _db.Agents.InsertOne(agent);
            return CreatedAtAction(nameof(GetAll), new { id = agent.Id }, agent);
        }
    }
}
