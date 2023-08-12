namespace TaskTracker.Domain.Events;

internal class TaskReassignedDomainEvent : DomainEvent
{
    public TaskReassignedDomainEvent(Task task)
    {
        Task = task;
    }
    
    public Task Task { get; }
}