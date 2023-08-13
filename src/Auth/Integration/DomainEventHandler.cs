using MediatR;

namespace Auth.Integration;

internal abstract class MessageBrokerDomainEventHandler<T> : INotificationHandler<T> where T : DomainEvent
{
    private readonly IKafkaMessageProducer _kafkaMessageProducer;

    protected MessageBrokerDomainEventHandler(IKafkaMessageProducer kafkaMessageProducer)
    {
        _kafkaMessageProducer = kafkaMessageProducer;
    }

    public async Task Handle(T domainEvent, CancellationToken cancellationToken)
    {
        if (MapToBehaviourMessage(domainEvent) is { } behaviourMessage)
            await _kafkaMessageProducer.Produce("ates-auth", behaviourMessage, cancellationToken);

        if (MapToStreamingMessage(domainEvent) is { } streamingMessage)
            await _kafkaMessageProducer.Produce("ates-auth-streaming", streamingMessage, cancellationToken);
    }

    protected abstract string? MapToBehaviourMessage(T domainEvent);
    protected abstract string? MapToStreamingMessage(T domainEvent);
}