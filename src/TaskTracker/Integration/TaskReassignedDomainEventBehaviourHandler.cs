using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskReassignedDomainEventBehaviourHandler : INotificationHandler<TaskReassignedDomainEvent>
{
    private readonly ILogger<TaskReassignedDomainEventBehaviourHandler> _logger;

    public TaskReassignedDomainEventBehaviourHandler(ILogger<TaskReassignedDomainEventBehaviourHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskReassignedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending business event {EventName}..", nameof(TaskReassignedDomainEvent));
        return Task.CompletedTask;
    }
}