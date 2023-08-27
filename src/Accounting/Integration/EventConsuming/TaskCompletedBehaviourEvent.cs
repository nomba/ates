namespace Accounting.Integration.EventConsuming;

public class TaskCompletedBehaviourEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Completed");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Behaviour;

    public TaskCompletedBehaviourEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public Guid AssigneeId { get; set; }
    }
}