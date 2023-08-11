namespace Auth.Domain.Events;

internal class PopugCreatedDomainEvent : DomainEvent
{
    public PopugCreatedDomainEvent(Popug popug)
    {
        Popug = popug;
    }
    
    public Popug Popug { get; }
}