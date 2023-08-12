namespace TaskTracker.Endpoints;

public class CreateTaskRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid AssigneeId { get; set; }
}