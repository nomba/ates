using MediatR;

namespace TaskTracker.Integration.EventConsuming;

internal abstract class MediatrBasedMessageBrokerEventConsumer<TMessageBrokerEvent> : INotificationHandler<TMessageBrokerEvent> where TMessageBrokerEvent : MessageBrokerEvent
{
    protected abstract Task Consume(TMessageBrokerEvent @event, CancellationToken cancellationToken);

    public async Task Handle(TMessageBrokerEvent notification, CancellationToken cancellationToken)
    {
        await Consume(notification, cancellationToken);
    }
}