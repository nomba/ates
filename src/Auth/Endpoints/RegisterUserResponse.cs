using Auth.Domain;

namespace Auth.Endpoints;

public class RegisterUserResponse
{
    public Guid PublicId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public RoleType Role { get; set; }
    public string? Email { get; set; }
}