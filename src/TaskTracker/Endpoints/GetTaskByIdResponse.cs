namespace TaskTracker.Endpoints;

public class GetTaskByIdResponse
{
    public string Title { get; set; }
    public string Description { get;  set;}
    public decimal Fee { get; set; }
    public decimal Reward { get; set; }

    public Domain.TaskStatus Status { get; set; }
    public Guid AssigneeId { get; set; }
    public IReadOnlyCollection<AssignmentDto> Assignments { get; set; }
}

public class AssignmentDto
{
    public Guid? CurrentPopugId { get; set; }
    public Guid NewPopugId { get; set; }
    public DateTime AssignedAt { get; set; }
}