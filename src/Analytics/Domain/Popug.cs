namespace Analytics.Domain;

public class Popug : Entity
{
    public Popug(string username, string fullName, RoleType initialRole)
    {
        Username = username;
        FullName = fullName;
        Role = initialRole;
        IsActive = true;
    }

    private Popug()
    {
        // For EF only
    }

    public string Username { get; }
    public string FullName { get; set; }
    public RoleType Role { get; set; }
    public string? Email { get; init; }
    public bool IsActive { get; set; }
    
    public Account? Account { get; set; }
}