using Microsoft.Extensions.Options;

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
        
        
        // TODO: Seeding if needed
    }
}