using System.Text.Json;
using MediatR;
using TaskTracker.Integration.EventConsuming;

namespace TaskTracker.Integration;

internal class KafkaMessageHandler : IKafkaMessageHandler
{
    private readonly IPublisher _publisher;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public KafkaMessageHandler(IPublisher publisher, IServiceScopeFactory serviceScopeFactory)
    {
        _publisher = publisher;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Handle(string topic, string message)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        if (JsonSerializer.Deserialize(message, typeof(MessageBrokerEvent)) is not MessageBrokerEvent messageBrokerEvent)
            throw new InvalidOperationException("Can't deserialize message to MessageBrokerEvent");

        var popugCreatedStreamingEvent = (PopugCreatedStreamingEvent) messageBrokerEvent;
        await _publisher.Publish(messageBrokerEvent);
    }
}