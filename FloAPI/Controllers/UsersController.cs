using Microsoft.AspNetCore.Mvc;
using FloApi.Data;
using FloApi.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using FloApi.Config;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Security.Cryptography;

namespace FloApi.Controllers
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
        [Authorize(Roles = "SysAdmin")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? role)
        {
            var filter = string.IsNullOrWhiteSpace(role)
                ? Builders<User>.Filter.Empty
                : Builders<User>.Filter.Eq(u => u.Role, role);

            var users = _db.Users.Find(filter).ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _db.Users.Find(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPost("agent")]
        [Authorize(Roles = "SysAdmin")]
        public IActionResult CreateAgent(User agent)
        {
            agent.Role = "Agent";
            agent.RegisteredAt = DateTime.UtcNow;
           // agent.TrialExpiresAt = DateTime.UtcNow.AddDays(_agentSettings.TrialDays); // or configurable
            agent.IsTrial = true;
            agent.IsActive = false; // activate after payment
            _db.Users.InsertOne(agent);
            //if (_agentSettings.SendMagicLinkOnCreate)
            //{
            //    // Generate a magic link (copy from AuthController)
            //    var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            //    agent.MagicToken = token;
            //    agent.MagicTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
            //    agent.MagicTokenUsed = false;

            //    _db.Users.ReplaceOne(u => u.Id == agent.Id, agent);

            //    var loginUrl = $"https://homepin.ca/login?token={token}";
            //    Console.WriteLine($"[MAGIC LINK] Sent to {agent.Email}: {loginUrl}");
            //}
            return CreatedAtAction(nameof(GetAll), new { id = agent.Id }, agent);
        }

        [HttpPost("client")]
        // [Authorize(Roles = "Agent")] // Add later
        public IActionResult CreateClient(User client)
        {
            client.Role = "Client";
            // Optional: set AgentId based on logged-in user later
            _db.Users.InsertOne(client);
            return CreatedAtAction(nameof(GetAll), new { id = client.Id }, client);
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            var user = _db.Users.Find(u => u.Id == userId).FirstOrDefault();
            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.TradeName,
                user.Email,
                user.Role
            });
        }

    }
}
