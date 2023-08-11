namespace Auth.Domain.Events;

internal class PopugDeactivatedDomainEvent : DomainEvent
{
    public PopugDeactivatedDomainEvent(Popug popug)
    {
        Popug = popug;
    }
    
    public Popug Popug { get; }
}