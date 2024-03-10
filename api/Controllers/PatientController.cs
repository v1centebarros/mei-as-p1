using System.Security.Claims;
using api.Data;
using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {

            //Access JWT token to get the role of the user

            SqlParameter role = new SqlParameter("@role", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            // run store procedure dbo.GetUserData
            var response = await _context.Database.SqlQuery<PatientResponse>($"EXECUTE dbo.GetUserData @role={role}").ToListAsync();
            return Ok(response);
        }
    }
}