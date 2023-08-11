using Auth.Domain.Events;
using MediatR;

namespace Auth.Integration;

internal class PopugDeactivatedDomainEventBehaviourHandler : INotificationHandler<PopugDeactivatedDomainEvent>
{
    private readonly ILogger<PopugDeactivatedDomainEventBehaviourHandler> _logger;

    public PopugDeactivatedDomainEventBehaviourHandler(ILogger<PopugDeactivatedDomainEventBehaviourHandler> logger)
    {
        _logger = logger;
    }
 
    public Task Handle(PopugDeactivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending business event {EventName}..", nameof(PopugCreatedDomainEvent));
        return Task.CompletedTask;
    }
}