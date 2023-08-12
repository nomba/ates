using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskCompletedDomainEventStreamingHandler : INotificationHandler<TaskCompletedDomainEvent>
{
    private readonly ILogger<TaskCompletedDomainEventStreamingHandler> _logger;

    public TaskCompletedDomainEventStreamingHandler(ILogger<TaskCompletedDomainEventStreamingHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending streaming event {EventName}..", nameof(TaskCreatedDomainEvent));
        return Task.CompletedTask;
    }
}