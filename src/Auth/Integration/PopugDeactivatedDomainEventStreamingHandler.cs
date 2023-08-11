using Auth.Domain.Events;
using MediatR;

namespace Auth.Integration;

internal class PopugDeactivatedDomainEventStreamingHandler : INotificationHandler<PopugDeactivatedDomainEvent>
{
    private readonly ILogger<PopugDeactivatedDomainEventStreamingHandler> _logger;

    public PopugDeactivatedDomainEventStreamingHandler(ILogger<PopugDeactivatedDomainEventStreamingHandler> logger)
    {
        _logger = logger;
    }
 
    public Task Handle(PopugDeactivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending streaming event {EventName}..", nameof(PopugCreatedDomainEvent));
        return Task.CompletedTask;
    }
}