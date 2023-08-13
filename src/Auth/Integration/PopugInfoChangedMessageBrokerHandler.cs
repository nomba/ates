using System.Text.Json;
using Auth.Domain.Events;

namespace Auth.Integration;

internal class PopugInfoChangedMessageBrokerHandler : MessageBrokerDomainEventHandler<PopugInfoChangedDomainEvent>
{
    public PopugInfoChangedMessageBrokerHandler(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override string? MapToBehaviourMessage(PopugInfoChangedDomainEvent domainEvent)
    {
        // There is no behaviour event for just info changed
        return default;
    }

    protected override string? MapToStreamingMessage(PopugInfoChangedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }
}