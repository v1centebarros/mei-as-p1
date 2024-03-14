using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(AppDbContext context, ILogger<PatientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PatientDTO> GetMe(string role, string id)
        {
            _logger.LogInformation($"GetMe method called with role: {role} and id: {id}");
            SqlParameter sqlRole = new SqlParameter("@role", "patient");
            SqlParameter sqlId = new SqlParameter("@id", id);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserById @role={sqlRole}, @id={sqlId}").ToListAsync();
            
            if (response.Count == 0)
            {
                _logger.LogWarning("GetMe method returned no results.");
                return null;
            }

            return response[0];
        }

        public async Task<PatientDTO> GetPatient(string role, string id)
        {
            _logger.LogInformation($"GetPatient method called with role: {role} and id: {id}");
            SqlParameter sqlRole = new SqlParameter("@role", role);
            SqlParameter userId = new SqlParameter("@id", id);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserById @role={sqlRole}, @id={userId}").ToListAsync();

            if (response.Count == 0)
            {
                _logger.LogWarning("GetPatient method returned no results.");
                return null;
            }

            return response[0];
        }

        public async Task<List<PatientDTO>> GetPatients(string role)
        {
            _logger.LogInformation($"GetPatients method called with role: {role}");
            SqlParameter sqlRole = new SqlParameter("@role", role);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserData @role={sqlRole}").ToListAsync();
            
            return response;
        }

        public async Task<PatientDTO> UpdateMe(string nameIdentifier,PatientDTO NewPatient)
        {
            _logger.LogInformation($"UpdateMe method called with nameIdentifier: {nameIdentifier}");
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == nameIdentifier);

            if (patient == null)
            {
                _logger.LogWarning("UpdateMe method failed. Patient not found.");
                return null;
            }

            // Update the patient's data
            patient.FullName = NewPatient.FullName;
            patient.ApplicationUser.Email = NewPatient.Email;
            patient.ApplicationUser.UserName = NewPatient.Email;
            patient.ApplicationUser.PhoneNumber = NewPatient.PhoneNumber;
            patient.MedicalRecord.TreatmentPlan = NewPatient.TreatmentPlan;
            patient.MedicalRecord.DiagnosisDetails = NewPatient.DiagnosisDetails;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
                {
                    _logger.LogError("UpdateMe method failed. Patient not found.");
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return NewPatient;
        }

        public async Task<PatientByHelpdeskDTO> UpdateByHelpdesk(PatientByHelpdeskDTO NewPatient, string id)
        {
            _logger.LogInformation($"UpdateByHelpdesk method called with id: {id}");
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (patient == null)
            {
                _logger.LogWarning("UpdateByHelpdesk method failed. Patient not found.");
                return null;
            }

            // Update the patient's data
            patient.FullName = NewPatient.FullName;
            patient.ApplicationUser.Email = NewPatient.Email;
            patient.ApplicationUser.UserName = NewPatient.Email;
            patient.MedicalRecord.MedicalRecordNumber = NewPatient.MedicalRecordNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
                {
                    _logger.LogError("UpdateByHelpdesk method failed. Patient not found.");
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return NewPatient;
        }

        public async Task<PatientByHelpdeskAuthorizedDTO> UpdateByHelpdeskWithAccessToken(PatientByHelpdeskAuthorizedDTO NewPatient, string id, string accessToken)
        {
            _logger.LogInformation($"UpdateByHelpdeskWithAccessToken method called with id: {id} and accessToken: {accessToken}");
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (patient == null)
            {
                _logger.LogWarning("UpdateByHelpdeskWithAccessToken method failed. Patient not found.");
                return null;
            }

            if (patient.MedicalRecord.AccessCode != HashAccessCode(accessToken))
            {
                _logger.LogWarning("UpdateByHelpdeskWithAccessToken method failed. Invalid access token.");
                return null;
            }

            // Update the patient's data
            patient.FullName = NewPatient.FullName;
            patient.ApplicationUser.Email = NewPatient.Email;
            patient.ApplicationUser.UserName = NewPatient.Email;
            patient.ApplicationUser.PhoneNumber = NewPatient.PhoneNumber;
            patient.MedicalRecord.MedicalRecordNumber = NewPatient.MedicalRecordNumber;
            patient.MedicalRecord.TreatmentPlan = NewPatient.TreatmentPlan;
            patient.MedicalRecord.DiagnosisDetails = NewPatient.DiagnosisDetails;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
                {
                    _logger.LogError("UpdateByHelpdeskWithAccessToken method failed. Patient not found.");
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return NewPatient;
        }

        private bool PatientExists(string id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        private string HashAccessCode(string accessCode)
        {
            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(accessCode));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}