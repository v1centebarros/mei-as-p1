using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController(IPatientRepository patientRepository) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> Get()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var response = await patientRepository.GetPatients(role);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await patientRepository.GetPatient(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, id);
            return Ok(response);
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var response = await patientRepository.GetMe(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] PatientDTO NewPatient)
        {
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var response = await patientRepository.UpdateMe(nameIdentifier, NewPatient);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(NewPatient);

        }


        [HttpPut("{id}")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> PutByHelpdesk([FromBody] PatientByHelpdeskDTO NewPatient, [Required] string id)
        {
            
            var response = await patientRepository.UpdateByHelpdesk(NewPatient, id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(NewPatient);
        }

        [HttpPut("{id}/AccessToken")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> PutByHelpdeskWithAccessToken([FromBody] PatientByHelpdeskAuthorizedDTO NewPatient, [Required] string id, [Required] string accessToken)
        {
            
            var response = await patientRepository.UpdateByHelpdeskWithAccessToken(NewPatient, id, accessToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(NewPatient);
        }

        
    }
}