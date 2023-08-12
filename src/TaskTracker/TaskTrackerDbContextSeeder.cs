using Microsoft.Extensions.Options;
using TaskTracker.Domain;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker;

internal class TaskTrackerDbContextSeeder
{
    private readonly TaskTrackerDbContext _dbContext;
    private readonly SeedingOptions _options;
    private readonly ILogger<TaskTrackerDbContext> _logger;

    public TaskTrackerDbContextSeeder(TaskTrackerDbContext dbContext, IOptions<SeedingOptions> options, ILogger<TaskTrackerDbContext> logger)
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

        var popug1 = new Popug("popu1", "Popu 1", RoleType.Employee);
        var popug2 = new Popug("popu2", "Popu 2", RoleType.Employee);
        var popug3 = new Popug("popu3", "Popu 3", RoleType.Employee);
        var popug4 = new Popug("poma1", "Poma 1", RoleType.Manager);
        var popug5 = new Popug("poma2", "Poma 2", RoleType.Manager);
        var popugs = new[] { popug1, popug2, popug3, popug4, popug5 };

        var task1 = new Domain.Task("Super important", "Super super super POPA", popug1, TaskPrice.RollDice, DateTime.Now);
        var task2 = new Domain.Task("Super important 2", "Super super super POPA 2", popug2, TaskPrice.RollDice, DateTime.Now);
        var task3 = new Domain.Task("Super important 2", "Super super super POPA 3", popug3, TaskPrice.RollDice, DateTime.Now);
        var tasks = new[] { task1, task2, task3 };

        _dbContext.Popugs.AddRange(popugs);
        _dbContext.Tasks.AddRange(tasks);
        
        await _dbContext.SaveChangesAsync();
    }
}