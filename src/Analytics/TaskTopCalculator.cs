using Microsoft.EntityFrameworkCore;

namespace Analytics;

internal class TaskTopCalculator : ITaskTopCalculator
{
    private readonly AnalyticsDbContext _dbContext;

    public TaskTopCalculator(AnalyticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<Domain.Task?> CalculateTaskTop(TaskTopType topType, CancellationToken cancellationToken)
    {
        return topType switch
        {
            TaskTopType.BestBargain => _dbContext.Tasks.OrderByDescending(t => t.Reward - t.Fee).FirstOrDefaultAsync(cancellationToken: cancellationToken),
            TaskTopType.BestRewarded => _dbContext.Tasks.OrderByDescending(t => t.Reward).FirstOrDefaultAsync(cancellationToken: cancellationToken),
            TaskTopType.MostExpensive => _dbContext.Tasks.OrderByDescending(t => t.Fee).FirstOrDefaultAsync(cancellationToken: cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(topType), topType, null)
        };
    }
}