namespace api.Models.DTOs
{
    public record class UserSession(string? Id, string? Name, string? Email, string? Role);
}