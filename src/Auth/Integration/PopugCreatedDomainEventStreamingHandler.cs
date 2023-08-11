using Auth.Domain.Events;
using MediatR;

namespace Auth.Integration;

internal class PopugCreatedDomainEventStreamingHandler : INotificationHandler<PopugCreatedDomainEvent>
{
    private readonly ILogger<PopugCreatedDomainEventStreamingHandler> _logger;

    public PopugCreatedDomainEventStreamingHandler(ILogger<PopugCreatedDomainEventStreamingHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(PopugCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
       _logger.LogInformation("Sending streaming event {EventName}..", nameof(PopugCreatedDomainEvent));
       return Task.CompletedTask;
    }
}