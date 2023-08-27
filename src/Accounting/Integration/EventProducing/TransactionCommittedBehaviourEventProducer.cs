using Accounting.Domain.Events;
using Accounting.Integration.EventConsuming;

namespace Accounting.Integration.EventProducing;

internal class TransactionCommittedBehaviourEventProducer : MessageBrokerDomainEventHandler<TransactionCommittedDomainEvent, TransactionCommittedBehaviourEvent>
{
    public TransactionCommittedBehaviourEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }

    protected override TransactionCommittedBehaviourEvent MapToMessageBrokerEvent(TransactionCommittedDomainEvent domainEvent)
    {
        return new TransactionCommittedBehaviourEvent(Guid.NewGuid(), DateTime.UtcNow, new TransactionCommittedBehaviourEvent.DataType
        {
            Id = domainEvent.Transaction.Id,
            Description = domainEvent.Transaction.Description,
            Credit = domainEvent.Transaction.Credit,
            Debit = domainEvent.Transaction.Debit
        });
    }

    protected override string GetTopic() => "account-life-cycle";
}