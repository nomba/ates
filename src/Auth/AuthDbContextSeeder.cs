using Microsoft.Extensions.Options;

namespace Auth;

internal class AuthDbContextSeeder
{
    private readonly AuthDbContext _dbContext;
    private readonly SeedingOptions _options;
    private readonly ILogger<AuthDbContextSeeder> _logger;

    public AuthDbContextSeeder(AuthDbContext dbContext, IOptions<SeedingOptions> options, ILogger<AuthDbContextSeeder> logger)
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