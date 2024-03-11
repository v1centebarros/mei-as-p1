

using api.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Patient
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } = null!;

        public required ApplicationUser ApplicationUser { get; set; }
        public string? FullName { get; set; }
        
        public MedicalRecord? MedicalRecord { get; set; }
    }
}