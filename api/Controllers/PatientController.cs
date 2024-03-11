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
            SqlParameter role = new SqlParameter("@role", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserData @role={role}").ToListAsync();
            return Ok(response);
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            SqlParameter role = new SqlParameter("@role", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            SqlParameter id = new SqlParameter("@id", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserById @role={role}, @id={id}").ToListAsync();
            return Ok(response[0]);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] PatientDTO NewPatient)
        {
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
           
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == nameIdentifier.Value);

            if (patient == null)
            {
                return NotFound();
            }

            // Update the patient's data
            patient.FullName = NewPatient.FullName;
            patient.ApplicationUser.Email = NewPatient.Email;
            patient.ApplicationUser.UserName = NewPatient.Email;
            patient.ApplicationUser.PhoneNumber = NewPatient.PhoneNumber;
            patient.MedicalRecord.TreatmentPlan = NewPatient.TreatmentPlan;
            patient.MedicalRecord.DiagnosisDetails = NewPatient.DiagnosisDetails;
            patient.MedicalRecord.AccessCode = NewPatient.AccessCode;
            
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(NewPatient);

        }

        private bool PatientExists(string id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}