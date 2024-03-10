using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    public class MedicalRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MedicalRecordNumber { get; set; } = Guid.NewGuid().ToString();
        public string TreatmentPlan { get; set; } = "No treatment plan";
        public string DiagnosisDetails { get; set; } = "No diagnosis details";
        public string AccessCode { get; set; } = Guid.NewGuid().ToString();
    }
}