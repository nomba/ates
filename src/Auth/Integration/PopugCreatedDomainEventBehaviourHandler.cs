using Auth.Domain.Events;
using MediatR;

namespace Auth.Integration;

internal class PopugCreatedDomainEventBehaviourHandler : INotificationHandler<PopugCreatedDomainEvent>
{
    private readonly ILogger<PopugCreatedDomainEventBehaviourHandler> _logger;

    public PopugCreatedDomainEventBehaviourHandler(ILogger<PopugCreatedDomainEventBehaviourHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(PopugCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending business event {EventName}..", nameof(PopugCreatedDomainEvent));
        return Task.CompletedTask;
    }
}