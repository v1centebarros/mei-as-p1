
namespace api.Models.DTOs
{
    public class PatientResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DiagnosisDetails { get; set; } = string.Empty;
        public string MedicalRecordNumber { get; set; } = string.Empty;
        public string TreatmentPlan { get; set; } = string.Empty;
        public string AccessCode { get; set; } = string.Empty;
    }
}