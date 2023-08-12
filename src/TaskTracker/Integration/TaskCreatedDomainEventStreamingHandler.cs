using MediatR;
using TaskTracker.Domain.Events;

namespace TaskTracker.Integration;

internal class TaskCreatedDomainEventStreamingHandler : INotificationHandler<TaskCreatedDomainEvent>
{
    private readonly ILogger<TaskCreatedDomainEventStreamingHandler> _logger;

    public TaskCreatedDomainEventStreamingHandler(ILogger<TaskCreatedDomainEventStreamingHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending streaming event {EventName}..", nameof(TaskCreatedDomainEvent));
        return Task.CompletedTask;
    }
}