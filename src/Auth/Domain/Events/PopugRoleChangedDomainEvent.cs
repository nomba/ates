namespace Auth.Domain.Events;

internal class PopugRoleChangedDomainEvent : DomainEvent
{
    public PopugRoleChangedDomainEvent(Popug popug, RoleType previousRole)
    {
        Popug = popug;
        PreviousRole = previousRole;
    }
    
    public Popug Popug { get; }
    public RoleType PreviousRole { get; }
}