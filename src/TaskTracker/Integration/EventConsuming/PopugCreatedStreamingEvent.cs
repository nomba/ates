using TaskTracker.Domain;

namespace TaskTracker.Integration.EventConsuming;

public class PopugCreatedStreamingEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Users", "Created");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Streaming;

    public PopugCreatedStreamingEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }
    
    // public PopugCreatedStreamingEvent(DataType data, Guid id, MessageBrokerEventName name, int version, DateTime createdAt) : base(data, id, name, version, createdAt)
    // {
    // }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public RoleType Role { get; set; }
    }
}