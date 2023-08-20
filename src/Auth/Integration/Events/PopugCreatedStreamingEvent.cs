namespace Auth.Integration.Events;

public class PopugCreatedStreamingEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Users", "Created");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Streaming;

    public PopugCreatedStreamingEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}