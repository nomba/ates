using Analytics.Domain;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace Analytics;

internal class AnalyticsDbContextSeeder
{
    private readonly AnalyticsDbContext _dbContext;
    private readonly SeedingOptions _options;
    private readonly ILogger<AnalyticsDbContextSeeder> _logger;

    public AnalyticsDbContextSeeder(AnalyticsDbContext dbContext, IOptions<SeedingOptions> options, ILogger<AnalyticsDbContextSeeder> logger)
    {
        _dbContext = dbContext;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (_options.RecreateDatabase)
        {
            _logger.LogInformation("Cleaning database..");
            await _dbContext.Database.EnsureDeletedAsync();
        }

        _logger.LogInformation("Creating database..");
        await _dbContext.Database.EnsureCreatedAsync();

        if (_options.FillTestData == false)
            return;

        // Test seeding

        var now = DateTime.UtcNow;

        // Popug

        var popug1 = new Popug("popu1", "Popu 1", RoleType.Employee);
        var popug2 = new Popug("popu2", "Popu 2", RoleType.Employee);
        var popug3 = new Popug("popu3", "Popu 3", RoleType.Employee);
        var popug4 = new Popug("poma1", "Poma 1", RoleType.Manager);
        var popug5 = new Popug("poma2", "Poma 2", RoleType.Manager);
        var popugs = new[] {popug1, popug2, popug3, popug4, popug5};

        // Task

        var task1 = new Domain.Task
        {
            Title = "Super important 1",
            Assignee = popug1,
            Fee = 10,
            Reward = 20
        };

        var task2 = new Domain.Task
        {
            Title = "Super important 2",
            Assignee = popug1,
            Fee = 15,
            Reward = 20
        };


        var task3 = new Domain.Task
        {
            Title = "Super important 3",
            Assignee = popug1,
            Fee = 20,
            Reward = 20
        };

        var tasks = new[] {task1, task2, task3};

        // Account

        var account1 = new Account {Balance = -10, Owner = popug1};
        var account2 = new Account {Balance = 10, Owner = popug4};

        var accounts = new[] {account1, account2};

        //

        _dbContext.Pogugs.AddRange(popugs);
        _dbContext.Tasks.AddRange(tasks);
        _dbContext.Accounts.AddRange(accounts);

        await _dbContext.SaveChangesAsync();

        // Top Items

        var topItem1 = new TaskTopItem {CalculatedAt = now.AddDays(-1), Task = task1.ToShortDescription(), TopType = TaskTopType.BestBargain};
        var topItem2 = new TaskTopItem {CalculatedAt = now, Task = task1.ToShortDescription(), TopType = TaskTopType.BestRewarded};
        var topItem3 = new TaskTopItem {CalculatedAt = now, Task = task1.ToShortDescription(), TopType = TaskTopType.MostExpensive};

        var topItems = new[] {topItem1, topItem2, topItem3};
        _dbContext.TaskTopItems.AddRange(topItems);

        // Money Stats Items

        var moneyStatsItem1 = new MoneyStatsItem
        {
            CalculatedAt = now.AddDays(-1), CurrentBalance = account1.Balance, PopugId = account1.Owner.Id, PopugRole = account1.Owner.Role, PopugFullName = account1.Owner.FullName
        };

        var moneyStatsItem2 = new MoneyStatsItem
        {
            CalculatedAt = now, CurrentBalance = account2.Balance, PopugId = account2.Owner.Id, PopugRole = account2.Owner.Role, PopugFullName = account2.Owner.FullName
        };

        var moneyStatsItems = new[] {moneyStatsItem1, moneyStatsItem2};
        _dbContext.MoneyStatsItems.AddRange(moneyStatsItems);

        await _dbContext.SaveChangesAsync();
    }
}