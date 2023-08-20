using Auth.Domain.Events;

namespace Auth.Integration.Events;

internal class PopugCreatedBehaviourEventProducer : MessageBrokerDomainEventHandler<PopugCreatedDomainEvent, PopugCreatedBehaviourEvent>
{
    public PopugCreatedBehaviourEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override PopugCreatedBehaviourEvent MapToMessageBrokerEvent(PopugCreatedDomainEvent domainEvent)
    {
        return new PopugCreatedBehaviourEvent(Guid.NewGuid(), DateTime.UtcNow, new PopugCreatedBehaviourEvent.DataType
        {
            Id = domainEvent.Popug.Id,
            Username = domainEvent.Popug.Username,
            FullName = domainEvent.Popug.FullName,
            Role = domainEvent.Popug.Role.ToString()
        });
    }

    protected override string GetTopic() => "popug-life-cycle";
}