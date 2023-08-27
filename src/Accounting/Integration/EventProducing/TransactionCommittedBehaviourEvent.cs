namespace Accounting.Integration.EventProducing;

public class TransactionCommittedBehaviourEvent : MessageBrokerEvent
{
    private static readonly MessageBrokerEventName EventName = new("Accounts", "TransactionCommitted");
    private static readonly MessageBrokerEventType EventType = MessageBrokerEventType.Behaviour;

    public TransactionCommittedBehaviourEvent(Guid id, DateTime createdAt, DataType data) : base(id, EventName, EventType, 1, createdAt, data)
    {
    }

    public class DataType
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
    }
}