using Microsoft.AspNetCore.Mvc;
using FloAPI.Data;
using FloAPI.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using FloAPI.Config;
using Microsoft.Extensions.Options;

namespace FloAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly MongoDbContext _db;
        private readonly JwtSettings _jwt;
        private readonly ConstantContactSettings _emailService;
        public AuthController(MongoDbContext db, IOptions<JwtSettings> jwtSettings, IOptions<ConstantContactSettings> emailService)
        {
            _db = db;
            _jwt = jwtSettings.Value;
            _emailService = emailService.Value;
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes(_jwt.Key);

            var claims = new List<System.Security.Claims.Claim>
                {
                    new("id", user.Id),
                    new(System.Security.Claims.ClaimTypes.Email, user.Email),
                    new(System.Security.Claims.ClaimTypes.Role, user.Role)
                };

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("send-link")]
        public IActionResult SendMagicLink([FromBody] string email)
        {
            var user = _db.Users.Find(u => u.Email == email).FirstOrDefault();
            if (user == null) return NotFound("User not found");

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.MagicToken = token;
            user.MagicTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
            user.MagicTokenUsed = false;

            _db.Users.ReplaceOne(u => u.Id == user.Id, user);

            var loginUrl = $"https://homepin.ca/login?token={token}";

            Console.WriteLine($"[DEBUG] Magic Link: {loginUrl}");

            return Ok("Login link sent (simulated)");
        }

        [HttpGet("login-with-token")]
        public IActionResult LoginWithToken([FromQuery] string token)
        {
            Console.WriteLine($"[DEBUG] Token received in query: {token}");
            var user = _db.Users.Find(u =>
                u.MagicToken == token &&
                u.MagicTokenUsed == false &&
                u.MagicTokenExpiresAt > DateTime.UtcNow).FirstOrDefault();

            if (user == null) return Unauthorized("Invalid or expired token");
            Console.WriteLine($"[LOGIN] User {user.DisplayName} ({user.Role}) logged in.");

            user.MagicTokenUsed = true;
            _db.Users.ReplaceOne(u => u.Id == user.Id, user);
            var jwtToken = GenerateJwtToken(user);
            return Ok(new
            {
                token = jwtToken,
                user = new
                {
                    user.Id,
                    user.TradeName,
                    user.Email,
                    user.Role
                }
            });
        }
    }
}
