namespace TaskTracker.Endpoints;

public class ListTasksResponseItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Fee { get; set; }
    public decimal Reward { get; set; }
    public Domain.TaskStatus Status { get; set; }
    public Guid AssigneeId { get; set; }
}