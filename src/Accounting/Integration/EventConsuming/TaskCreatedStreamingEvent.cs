namespace Accounting.Integration.EventConsuming;

public class TaskCreatedStreamingEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Created");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Streaming;

    public TaskCreatedStreamingEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 2, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
        public double Reward { get; set; }
        public TaskStatus Status { get; set; }
        public Guid AssigneeId { get; set; }
    }
}