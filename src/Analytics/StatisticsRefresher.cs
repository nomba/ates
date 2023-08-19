using Cronos;
using Microsoft.EntityFrameworkCore;

namespace Analytics;

public class StatisticsRefresher : BackgroundService
{
    private const string Schedule = "* * * * *"; // every minute
    // private const string Schedule = "0 0 * * *"; // every night 00:00
    private readonly CronExpression _cron;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StatisticsRefresher(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cron = CronExpression.Parse(Schedule);
    }

    // Based on https://stackoverflow.com/questions/71883959/how-to-start-my-worker-service-on-the-hour
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            var nextUtc = _cron.GetNextOccurrence(utcNow);
            
            if (nextUtc is null)
                return;
            
            await Task.Delay(nextUtc.Value - utcNow, stoppingToken);
            
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
            var taskTopCalculator = scope.ServiceProvider.GetRequiredService<ITaskTopCalculator>();

            await RefreshStatistics(dbContext, taskTopCalculator, stoppingToken);
        }
    }
    
    private static async Task RefreshStatistics(AnalyticsDbContext dbContext, ITaskTopCalculator taskTopCalculator, CancellationToken cancellationToken)
    {
        await RefreshTaskTopStatistics(dbContext, taskTopCalculator, cancellationToken);
        await RefreshMoneyStatistics(dbContext, cancellationToken);
    }
    
    private static async Task RefreshTaskTopStatistics(AnalyticsDbContext dbContext,  ITaskTopCalculator taskTopCalculator, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        
        if (await taskTopCalculator.CalculateTaskTop(TaskTopType.BestRewarded, cancellationToken) is { } bestRewardedTask)
            dbContext.TaskTopItems.Add(new TaskTopItem {CalculatedAt = now, Task = bestRewardedTask.ToShortDescription(), TopType = TaskTopType.BestRewarded});

        if (await taskTopCalculator.CalculateTaskTop(TaskTopType.MostExpensive, cancellationToken) is { } mostExpensiveTask)
            dbContext.TaskTopItems.Add(new TaskTopItem {CalculatedAt = now, Task = mostExpensiveTask.ToShortDescription(), TopType = TaskTopType.MostExpensive});

        if (await taskTopCalculator.CalculateTaskTop(TaskTopType.BestBargain, cancellationToken)is { } bestBargainTask)
            dbContext.TaskTopItems.Add(new TaskTopItem {CalculatedAt = now, Task = bestBargainTask.ToShortDescription(), TopType = TaskTopType.BestBargain});

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task RefreshMoneyStatistics(AnalyticsDbContext dbContext, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var accounts = await dbContext.Accounts
            .Select(account => new MoneyStatsItem
            {
                CalculatedAt = now,
                CurrentBalance = account.Balance,
                PopugFullName = account.Owner.FullName,
                PopugRole = account.Owner.Role
            })
            .ToListAsync(cancellationToken);
       
        dbContext.MoneyStatsItems.AddRange(accounts);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}