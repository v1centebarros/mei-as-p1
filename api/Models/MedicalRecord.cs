using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string TreatmentPlan { get; set; }
        public string DiagnosisDetails { get; set; }
        public string AccessCode { get; set; }
    }
}