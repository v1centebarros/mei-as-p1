
using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs
{
    public class PatientDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string DiagnosisDetails { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string MedicalRecordNumber { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string TreatmentPlan { get; set; } = string.Empty;
    }
}