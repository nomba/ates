namespace Accounting.Integration.EventConsuming;

public class TaskCreatedStreamingEventV2 : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Created");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Streaming;

    public TaskCreatedStreamingEventV2(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 2, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string JiraId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
        public double Reward { get; set; }
        public string Status { get; set; }
        public Guid AssigneeId { get; set; }
    }
}