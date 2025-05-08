using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FloApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping endpoint was called at {Time}", DateTime.UtcNow);
            return Ok("Pong!");
        }
    }
}
