using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}