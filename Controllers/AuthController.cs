using Microsoft.AspNetCore.Mvc;
using FloAPI.Data;
using FloAPI.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace FloAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MongoDbContext _db;

        public AuthController(MongoDbContext db)
        {
            _db = db;
        }

        [HttpPost("send-link")]
        public IActionResult SendMagicLink([FromBody] string email)
        {
            var agent = _db.Agents.Find(a => a.Email == email).FirstOrDefault();
            if (agent == null) return NotFound("Agent not found");

            // Generate one-time token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            agent.MagicToken = token;
            agent.MagicTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
            agent.MagicTokenUsed = false;

            _db.Agents.ReplaceOne(a => a.Id == agent.Id, agent);

            // Replace this with actual email sending logic
            var loginUrl = $"https://homepin.ca/login?token={token}";

            Console.WriteLine($"[DEBUG] Magic Link: {loginUrl}");

            return Ok("Login link sent (simulated)");
        }

        [HttpGet("login-with-token")]
        public IActionResult LoginWithToken([FromQuery] string token)
        {
            var agent = _db.Agents.Find(a =>
                a.MagicToken == token &&
                a.MagicTokenUsed == false &&
                a.MagicTokenExpiresAt > DateTime.UtcNow).FirstOrDefault();

            if (agent == null) return Unauthorized("Invalid or expired token");

            agent.MagicTokenUsed = true;
            _db.Agents.ReplaceOne(a => a.Id == agent.Id, agent);

            // For now, return agent info (later return JWT)
            return Ok(new
            {
                message = "Logged in successfully",
                agent.Id,
                agent.TradeName,
                agent.Email
            });
        }
    }
}
