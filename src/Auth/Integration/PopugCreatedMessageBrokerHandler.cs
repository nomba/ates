using System.Text.Json;
using Auth.Domain.Events;

namespace Auth.Integration;

internal class PopugCreatedMessageBrokerHandler : MessageBrokerDomainEventHandler<PopugCreatedDomainEvent>
{
    public PopugCreatedMessageBrokerHandler(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override string? MapToBehaviourMessage(PopugCreatedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }

    protected override string? MapToStreamingMessage(PopugCreatedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }
}