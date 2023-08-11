namespace Auth.Domain.Events;

internal class PopugInfoChangedDomainEvent : DomainEvent
{
    public PopugInfoChangedDomainEvent(Popug popug)
    {
        Popug = popug;
    }
    
    public Popug Popug { get; }
}