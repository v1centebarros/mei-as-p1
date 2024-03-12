
using api.Models.DTOs;

namespace api.Models.Contracts {
    public interface IPatientRepository
    {

        Task<List<PatientDTO>> GetPatients(string role);
        Task<PatientDTO> GetPatient(string role, string id);
        Task<PatientDTO> GetMe(string role, string id);
        Task<PatientDTO> UpdateMe(string nameIdentifier, PatientDTO NewPatient);

        Task<PatientByHelpdeskDTO> UpdateByHelpdesk(PatientByHelpdeskDTO NewPatient, string id);
        Task<PatientByHelpdeskAuthorizedDTO> UpdateByHelpdeskWithAccessToken(PatientByHelpdeskAuthorizedDTO NewPatient, string id, string accessToken);        
    }
}