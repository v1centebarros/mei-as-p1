using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationUser : IdentityUser
    {
        
        public string? FullName { get; set; }
        
        public string? MedicalRecordNumber { get; set; }

        public string? TreatmentPlan { get; set; }

        public string? AccessCode { get; set; }
    }
}