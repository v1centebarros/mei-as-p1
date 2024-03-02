using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await userAccount.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccount.Login(loginDTO);
            return Ok(response);
        }
    }
}