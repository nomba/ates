using Auth.Domain.Events;

namespace Auth.Domain;

public class Popug : Entity
{
    public Popug(string username, string fullName, RoleType initialRole)
    {
        Username = username;
        FullName = fullName;
        Role = initialRole;
        IsActive = true;
        
        _domainEvents.Add(new PopugCreatedDomainEvent(this));
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

    public void ChangeInfo(string newFullName)
    {
        FullName = newFullName;
        _domainEvents.Add(new PopugInfoChangedDomainEvent(this));
    }

    public void ChangeRole(RoleType newRole)
    {
        var previousRole = Role;
        Role = newRole;
        
        _domainEvents.Add(new PopugRoleChangedDomainEvent(this, previousRole));
    }
    
    public void Deactivate()
    {
        IsActive = false;
        _domainEvents.Add(new PopugDeactivatedDomainEvent(this));
    }
}