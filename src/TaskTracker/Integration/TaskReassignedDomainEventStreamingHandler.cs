using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskReassignedDomainEventStreamingHandler : INotificationHandler<TaskReassignedDomainEvent>
{
    private readonly ILogger<TaskReassignedDomainEventStreamingHandler> _logger;

    public TaskReassignedDomainEventStreamingHandler(ILogger<TaskReassignedDomainEventStreamingHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskReassignedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending streaming event {EventName}..", nameof(TaskReassignedDomainEvent));
        return Task.CompletedTask;
    }
}