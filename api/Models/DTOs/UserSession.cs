namespace api.Models.DTOs
{
    public record class UserSession(string? Id, string? FullName, string? Email, string? TreatmentPlan,string? MedicalRecordNumber, string? AccessCode, string? Role);
}