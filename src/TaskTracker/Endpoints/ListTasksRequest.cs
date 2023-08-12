namespace TaskTracker.Endpoints;

public class ListTasksRequest
{
    public Domain.TaskStatus? OnlyStatus { get; set; } 
    public Guid? OnlyAssignee { get; set; } 
}