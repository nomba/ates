using Accounting.Domain;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace Accounting;

internal class AccountingDbContextSeeder
{
    private readonly AccountingDbContext _dbContext;
    private readonly SeedingOptions _options;
    private readonly ILogger<AccountingDbContextSeeder> _logger;

    public AccountingDbContextSeeder(AccountingDbContext dbContext, IOptions<SeedingOptions> options, ILogger<AccountingDbContextSeeder> logger)
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