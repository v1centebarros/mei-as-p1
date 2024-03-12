using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserAuth userAuth) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await userAuth.Create(userDTO);

            if(!response.Flag)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAuth.Login(loginDTO);

            if(!response.Flag)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}