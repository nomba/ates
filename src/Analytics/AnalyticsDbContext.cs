using Analytics.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Analytics;

public class AnalyticsDbContext : DbContext
{
    private readonly IPublisher _integrationEventPublisher;
    private readonly ILogger<AnalyticsDbContext> _logger;

    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options, IPublisher integrationEventPublisher, ILogger<AnalyticsDbContext> logger) : base(options)
    {
        _integrationEventPublisher = integrationEventPublisher;
        _logger = logger;
    }

    public DbSet<Popug> Pogugs { get; set; }
    public DbSet<Domain.Task> Tasks { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<TaskTopItem> TaskTopItems { get; set; }
    public DbSet<MoneyStatsItem> MoneyStatsItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Pogug

        modelBuilder.Entity<Popug>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username);
            entity.Property(e => e.FullName);
            entity.Property(e => e.Role).HasConversion(v => v.ToString(), v => (RoleType) Enum.Parse(typeof(RoleType), v));
            entity.Property(e => e.Email);
            entity.Property(e => e.IsActive);
            entity.HasOne(e => e.Account).WithOne(e => e.Owner).HasForeignKey<Account>(e => e.Id).IsRequired();
            entity.Ignore(e => e.DomainEvents);
        });

        // Task

        modelBuilder.Entity<Domain.Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title);
            entity.Property(e => e.Fee);
            entity.Property(e => e.Reward);
            entity.HasOne(e => e.Assignee).WithMany();
            entity.Ignore(e => e.DomainEvents);
        });

        // Account

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Balance);
            entity.Ignore(e => e.DomainEvents);
        });

        // Top

        modelBuilder.Entity<TaskTopItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Task);
            entity.Property(e => e.TopType).HasConversion(v => v.ToString(), v => (TaskTopType) Enum.Parse(typeof(TaskTopType), v));
            entity.Property(e => e.CalculatedAt);
        });

        // Money Stats

        modelBuilder.Entity<MoneyStatsItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PopugId);
            entity.Property(e => e.PopugRole).HasConversion(v => v.ToString(), v => (RoleType) Enum.Parse(typeof(RoleType), v));
            entity.Property(e => e.PopugFullName);
            entity.Property(e => e.CurrentBalance);
            entity.Property(e => e.CalculatedAt);
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