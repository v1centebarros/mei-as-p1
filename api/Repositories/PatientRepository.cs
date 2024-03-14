
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace api.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PatientDTO> GetMe(string role, string id)
        {
            SqlParameter sqlRole = new SqlParameter("@role", "patient");
            SqlParameter sqlId = new SqlParameter("@id", id);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserById @role={sqlRole}, @id={sqlId}").ToListAsync();
            
            if (response.Count == 0)
            {
                return null;
            }

            return response[0];
        }

        public async Task<PatientDTO> GetPatient(string role, string id)
        {
            SqlParameter sqlRole = new SqlParameter("@role", role);
            SqlParameter userId = new SqlParameter("@id", id);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserById @role={sqlRole}, @id={userId}").ToListAsync();

            if (response.Count == 0)
            {
                return null;
            }

            return response[0];
            
        }

        public async Task<List<PatientDTO>> GetPatients(string role)
        {
            SqlParameter sqlRole = new SqlParameter("@role", role);
            var response = await _context.Database.SqlQuery<PatientDTO>($"EXECUTE dbo.GetUserData @role={sqlRole}").ToListAsync();
            
            return response;
        }
        public async Task<PatientDTO> UpdateMe(string nameIdentifier,PatientDTO NewPatient)
        {
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == nameIdentifier);

            if (patient == null)
            {
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
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (patient == null)
            {
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
            var patient = await _context.Patients
                .Include(p => p.ApplicationUser)
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (patient == null)
            {
                return null;
            }

            if (patient.MedicalRecord.AccessCode != HashAccessCode(accessToken))
            {
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