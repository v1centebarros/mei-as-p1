using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;



        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string TreatmentPlan { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string DiagnosisDetails { get; set; } = string.Empty;
    }
}
