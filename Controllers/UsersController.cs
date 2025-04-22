using Microsoft.AspNetCore.Mvc;
using FloAPI.Data;
using FloAPI.Models;
using MongoDB.Driver;

namespace FloAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly MongoDbContext _db;

        public UsersController(MongoDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _db.Users.Find(_ => true).ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            _db.Users.InsertOne(user);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }
    }
}
