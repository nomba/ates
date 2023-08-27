using TaskTracker.Domain;

namespace TaskTracker.Integration.EventConsuming;

public class RoleChangedBehaviourEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Users", "RoleChanged");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Behaviour;

    public RoleChangedBehaviourEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid PopugId { get; }
        public RoleType NewRole { get; }
        public RoleType PreviousRole { get; }
    }
}