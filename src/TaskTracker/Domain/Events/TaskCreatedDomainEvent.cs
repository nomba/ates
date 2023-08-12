namespace TaskTracker.Domain.Events;

internal class TaskCreatedDomainEvent : DomainEvent
{
    public TaskCreatedDomainEvent(Task task)
    {
        Task = task;
    }
    
    public Task Task { get; }
}