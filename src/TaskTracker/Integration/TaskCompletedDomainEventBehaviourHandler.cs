using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskCompletedDomainEventBehaviourHandler : INotificationHandler<TaskCompletedDomainEvent>
{
    private readonly ILogger<TaskCompletedDomainEventBehaviourHandler> _logger;

    public TaskCompletedDomainEventBehaviourHandler(ILogger<TaskCompletedDomainEventBehaviourHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending business event {EventName}..", nameof(TaskCompletedDomainEvent));
        return Task.CompletedTask;
    }
}