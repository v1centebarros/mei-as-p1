using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs
{
    public class PatientByHelpdeskDTO
    {
        public string FullName { get; set; } = string.Empty;
        
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        public string MedicalRecordNumber { get; set; } = string.Empty;
    }
}