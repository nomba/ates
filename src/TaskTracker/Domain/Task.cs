using TaskTracker.Domain.Events;

namespace TaskTracker.Domain;

public class Task : Entity
{
    private readonly List<Assignment> _assignments = new();

    public Task(string title, string description, Popug assignee, TaskPrice price, DateTime submittedAt)
    {
        CanBeAssignedTo(assignee, true);

        Title = title;
        Description = description;
        Assignee = assignee;
        Price = price;
    
        Status = TaskStatus.Submitted;
        _assignments.Add(new Assignment(null, assignee, submittedAt));
        _domainEvents.Add(new TaskCreatedDomainEvent(this));
    }

    private Task()
    { 
        // For EF        
    }

    public string Title { get; }
    public string Description { get; }
    public TaskPrice Price { get; }
    
    public TaskStatus Status { get; private set; }
    public Popug Assignee { get; private set; }
    public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

    public static bool CanBeAssignedTo(Popug popug, bool throwDomainExceptionIfCannot)
    {
        var canBeAssignee = popug.Role == RoleType.Employee;

        if (canBeAssignee == false && throwDomainExceptionIfCannot)
            throw new DomainException("Only regular popug can be assigned to task. Not Admin or Manager");

        return canBeAssignee;
    }

    public void Reassign(Popug newAssignee, DateTime reassignedAt)
    {
        CanBeAssignedTo(newAssignee, true);

        var current = Assignee;
        Assignee = newAssignee;
        _assignments.Add(new Assignment(current, newAssignee, reassignedAt));
        
        _domainEvents.Add(new TaskReassignedDomainEvent(this));
    }

    public void Complete()
    {
        if (Status == TaskStatus.Done)
            throw new DomainException("Task already completed. It cannot be completed twice.");

        Status = TaskStatus.Done;
        _domainEvents.Add(new TaskCompletedDomainEvent(this));
    }
}