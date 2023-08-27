using Accounting.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Accounting;

public class AccountingDbContext : DbContext
{
    private readonly IPublisher _integrationEventPublisher;
    private readonly ILogger<AccountingDbContext> _logger;
    
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options, IPublisher integrationEventPublisher, ILogger<AccountingDbContext> logger) : base(options)
    {
        _integrationEventPublisher = integrationEventPublisher;
        _logger = logger;
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Domain.Task> Tasks { get; set; }
    public DbSet<Popug> Pogugs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Pogug

        modelBuilder.Entity<Popug>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username);
            entity.Property(e => e.FullName);
            entity.Property(e => e.Role).HasConversion(v => v.ToString(), v => (RoleType)Enum.Parse(typeof(RoleType), v));
            entity.Property(e => e.Email);
            entity.Property(e => e.IsActive);
            entity.Ignore(e => e.DomainEvents);
        });
    }
    
    public override int SaveChanges()
    {
        throw new NotImplementedException($"This overload does not implement domain event publishing. Use {nameof(SaveChangesAsync)} instead.");
    }
    
    
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new NotImplementedException($"This overload does not implement domain event publishing. Use {nameof(SaveChangesAsync)} instead.");
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // NOTE: Publish domain events ONLY for integrations.
        // Therefore we do not need publishing before transaction committed

        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any()).ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // Commit transaction
        var result = await base.SaveChangesAsync(cancellationToken);

        // Clear all events AFTER transaction committed
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        // Publish integration events by domain ones. AFTER transaction committed
        // Simple scenario: Transaction is not rollback if integration failed
        
        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _integrationEventPublisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Integration failed. Unable to publish {EventName}", domainEvent.GetType().ShortDisplayName());
            }
        }

        return result;
    }
}