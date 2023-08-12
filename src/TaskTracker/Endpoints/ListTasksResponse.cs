namespace TaskTracker.Endpoints;

public class ListTasksResponse
{
    public IReadOnlyCollection<ListTasksResponseItem> Items { get; set; }
}