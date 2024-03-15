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
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository, ILogger<PatientController> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Get method called.");
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var response = await _patientRepository.GetPatients(role);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Get method called with id: {id}");
            var response = await _patientRepository.GetPatient(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, id);
            return Ok(response);
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            _logger.LogInformation("GetMe method called.");
            var response = await _patientRepository.GetMe(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] PatientDTO NewPatient)
        {
            _logger.LogInformation("Put method called.");
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var response = await _patientRepository.UpdateMe(nameIdentifier, NewPatient);

            if (response == null)
            {
                _logger.LogWarning("Update failed. Patient not found.");
                return NotFound();
            }

            return Ok(NewPatient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> PutByHelpdesk([FromBody] PatientByHelpdeskDTO NewPatient, [Required] string id)
        {
            _logger.LogInformation($"PutByHelpdesk method called with id: {id}");
            var response = await _patientRepository.UpdateByHelpdesk(NewPatient, id);

            if (response == null)
            {
                _logger.LogWarning("Update by helpdesk failed. Patient not found.");
                return NotFound();
            }

            return Ok(NewPatient);
        }

        [HttpPut("{id}/AccessToken")]
        [Authorize(Roles = "helpdesk")]
        public async Task<IActionResult> PutByHelpdeskWithAccessToken([FromBody] PatientByHelpdeskAuthorizedDTO NewPatient, [Required] string id, [Required] string accessToken)
        {
            _logger.LogInformation($"PutByHelpdeskWithAccessToken method called with id: {id} and accessToken: {accessToken}");
            var response = await _patientRepository.UpdateByHelpdeskWithAccessToken(NewPatient, id, accessToken);

            if (response == null)
            {
                _logger.LogWarning("Update by helpdesk with access token failed. Patient not found.");
                return NotFound();
            }

            return Ok(NewPatient);
        }

        [HttpGet("/error")]
        public IActionResult Error()
        {
            _logger.LogError("Error method called.");
            throw new Exception("This is a test exception.");
        }
    }
}