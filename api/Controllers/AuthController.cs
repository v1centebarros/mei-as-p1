using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserAuth _userAuth;

        public AuthController(IUserAuth userAuth, ILogger<AuthController> logger)
        {
            _userAuth = userAuth;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            _logger.LogInformation("Register method called");
            var response = await _userAuth.Create(userDTO);

            if(!response.Flag)
            {
                _logger.LogWarning("Registration failed for user: {User}", userDTO.FullName);
                return BadRequest(response);
            }

            _logger.LogInformation("Registration successful for user: {User}", userDTO.FullName);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            _logger.LogInformation("Login method called");
            var response = await _userAuth.Login(loginDTO);

            if(!response.Flag)
            {
                _logger.LogWarning("Login failed for user: {User}", loginDTO.Email);
                return BadRequest(response);
            }

            _logger.LogInformation("Login successful for user: {User}", loginDTO.Email);
            return Ok(response);
        }
    }
}