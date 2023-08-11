using Auth.Domain;

namespace Auth.Endpoints;

public class ListUsersResponseItem
{
    public Guid Pid { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public RoleType Role { get;  set; }
    public string? Email { get; set; }
    public bool IsActive { get;  set; }
}