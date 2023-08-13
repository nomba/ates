using System.Text.Json;
using Auth.Domain.Events;

namespace Auth.Integration;

internal class PopugDeactivatedMessageBrokerHandler : MessageBrokerDomainEventHandler<PopugDeactivatedDomainEvent>
{
    public PopugDeactivatedMessageBrokerHandler(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override string? MapToBehaviourMessage(PopugDeactivatedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }

    protected override string? MapToStreamingMessage(PopugDeactivatedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }
}