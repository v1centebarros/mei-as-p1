using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = default!;

        public DbSet<Patient> Patients { get; set; } = default!;
    }
}