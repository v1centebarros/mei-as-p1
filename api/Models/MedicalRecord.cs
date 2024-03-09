using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    public class MedicalRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string TreatmentPlan { get; set; }
        public string DiagnosisDetails { get; set; }
        public string AccessCode { get; set; }
    }
}