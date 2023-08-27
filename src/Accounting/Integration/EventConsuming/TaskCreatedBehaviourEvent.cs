namespace Accounting.Integration.EventConsuming;

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
        public Guid AssigneeId { get; set; }
    }
}