namespace Auth.Integration.Events;

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
        public string NewRole { get; }
        public string PreviousRole { get; }
    }
}