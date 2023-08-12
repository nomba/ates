namespace TaskTracker.Domain;

public class Popug : Entity
{
    // NOTE: A public constructor needs only for seeding purpose
    // Popug appears in TaskTracker context only through streaming
    // DDD implementation is good in restriction term,
    // e.g. I would create private constructor and forbade creating Popug at all in TaskTracker
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
    public RoleType Role { get; private set; }
    public string? Email { get; init; }
    public bool IsActive { get; private set; }
}