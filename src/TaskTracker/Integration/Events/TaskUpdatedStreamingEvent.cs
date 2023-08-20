namespace TaskTracker.Integration.Events;

public class TaskUpdatedStreamingEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Updated");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Streaming;

    public TaskUpdatedStreamingEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Title { get; }
        public string Description { get; }
        public double Price { get; }
        public string Status { get;  set; }
        public Guid AssigneeId { get; set; }
    }
}