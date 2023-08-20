using Auth.Domain.Events;

namespace Auth.Integration.Events;

internal class PopugCreatedStreamingEventProducer : MessageBrokerDomainEventHandler<PopugCreatedDomainEvent, PopugCreatedStreamingEvent>
{
    public PopugCreatedStreamingEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override PopugCreatedStreamingEvent MapToMessageBrokerEvent(PopugCreatedDomainEvent domainEvent)
    {
        return new PopugCreatedStreamingEvent(Guid.NewGuid(), DateTime.UtcNow, new PopugCreatedStreamingEvent.DataType
        {
            Id = domainEvent.Popug.Id,
            Username = domainEvent.Popug.Username,
            FullName = domainEvent.Popug.FullName,
            Role = domainEvent.Popug.Role.ToString()
        });
    }

    protected override string GetTopic() => "popug-user-streaming";
}