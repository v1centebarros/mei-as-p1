using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string? FullName { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
    }
}