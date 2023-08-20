namespace TaskTracker.Integration.Events;

public class TaskCreatedBehaviourEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Created");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Behaviour;

    public TaskCreatedBehaviourEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
        public double Reward { get; set; }
        public string Status { get; set; }
        public Guid AssigneeId { get; set; }
    }
}