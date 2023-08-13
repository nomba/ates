using System.Text.Json;
using Auth.Domain.Events;

namespace Auth.Integration;

internal class PopugRoleChangedMessageBrokerHandler : MessageBrokerDomainEventHandler<PopugRoleChangedDomainEvent>
{
    public PopugRoleChangedMessageBrokerHandler(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }
    
    protected override string? MapToBehaviourMessage(PopugRoleChangedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }

    protected override string? MapToStreamingMessage(PopugRoleChangedDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }
}