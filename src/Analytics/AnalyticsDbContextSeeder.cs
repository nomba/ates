using Analytics.Domain;
using Microsoft.Extensions.Options;

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

        // Seed System Admin
        if (_options.RecreateDatabase)
        {
            _dbContext.Pogugs.Add(new Popug(_options.SuperPopug, "Super Popug", RoleType.Administrator));
            await _dbContext.SaveChangesAsync();
        }
        
        // TODO: Seeding if needed
    }
}