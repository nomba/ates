namespace TaskTracker.Integration.Events;

public class TaskReassignedBehaviourEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Tasks", "Reassigned");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Behaviour;

    public TaskReassignedBehaviourEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public Guid NewAssigneeId { get; set; }
        public Guid PreviousAssigneeId { get; set; }
    }
}