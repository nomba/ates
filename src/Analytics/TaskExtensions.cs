namespace Analytics;

public static class TaskExtensions
{
    public static string ToShortDescription(this Domain.Task task)
    {
        return $"Id={task.Id}:Title={task.Title}:Fee={task.Fee}:Reward={task.Reward}";
    }
}