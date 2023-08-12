namespace TaskTracker.Domain.Events;

internal class TaskCompletedDomainEvent : DomainEvent
{
    public TaskCompletedDomainEvent(Task task)
    {
        Task = task;
    }
    
    public Task Task { get; }
}