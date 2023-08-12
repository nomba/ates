using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskCreatedDomainEventBehaviourHandler : INotificationHandler<TaskCreatedDomainEvent>
{
    private readonly ILogger<TaskCreatedDomainEventBehaviourHandler> _logger;

    public TaskCreatedDomainEventBehaviourHandler(ILogger<TaskCreatedDomainEventBehaviourHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending business event {EventName}..", nameof(TaskCreatedDomainEvent));
        return Task.CompletedTask;
    }
}