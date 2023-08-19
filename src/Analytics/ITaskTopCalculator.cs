namespace Analytics;

public interface ITaskTopCalculator
{
    Task<Domain.Task?> CalculateTaskTop(TaskTopType topType, CancellationToken cancellationToken);
}