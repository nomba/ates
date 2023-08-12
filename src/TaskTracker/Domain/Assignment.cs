namespace TaskTracker.Domain;

public class Assignment : Entity
{
    public Assignment(Popug? currentPopug, Popug newPopug, DateTime assignedAt)
    {
        CurrentPopug = currentPopug;
        NewPopug = newPopug;
        AssignedAt = assignedAt;
    }

    private Assignment()
    {
        // For EF
    }
    
    public Popug? CurrentPopug { get; }
    public Popug NewPopug { get; }
    public DateTime AssignedAt { get; }
}