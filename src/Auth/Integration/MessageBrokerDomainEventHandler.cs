using System.Text.Json;
using MediatR;

namespace Auth.Integration;

internal abstract class MessageBrokerDomainEventHandler<TDomainEvent, TMessageBrokerEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
    where TMessageBrokerEvent : MessageBrokerEvent
{
    private readonly IKafkaMessageProducer _kafkaMessageProducer;

    public MessageBrokerDomainEventHandler(IKafkaMessageProducer kafkaMessageProducer)
    {
        _kafkaMessageProducer = kafkaMessageProducer;
    }

    public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (MapToMessageBrokerEvent(domainEvent) is not { } messageBrokerEvent)
            throw new InvalidOperationException($"Unable to map {typeof(TDomainEvent)} to {typeof(TMessageBrokerEvent)}");

        // Serialize to Json and validate with Schema Registry
        var eventJson = JsonSerializer.Serialize(messageBrokerEvent);

        // TODO: Fix `Json.Schema.JsonSchemaException: Cannot resolve custom meta-schema.  Make sure meta-schemas are registered in the global registry.`
        // SchemaRegistry.Validator.ValidateEvent(eventJson, messageBrokerEvent.Name.Domain, messageBrokerEvent.Name.Value, messageBrokerEvent.Version);

        await _kafkaMessageProducer.Produce(GetTopic(), eventJson, cancellationToken);
    }

    protected abstract TMessageBrokerEvent MapToMessageBrokerEvent(TDomainEvent domainEvent);
    protected abstract string GetTopic();
}