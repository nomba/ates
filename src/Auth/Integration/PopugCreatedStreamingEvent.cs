using Auth.Domain;

namespace Auth.Integration;

public class PopugCreatedStreamingEvent : StreamingEvent
{
    public PopugCreatedStreamingEvent(Popug popug)
    {
        Popug = popug;
    }
    
    public Popug Popug { get; }
}