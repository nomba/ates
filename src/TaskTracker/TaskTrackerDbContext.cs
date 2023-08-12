using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaskTracker.Domain;

namespace TaskTracker;

public class TaskTrackerDbContext : DbContext
{
    private readonly IPublisher _integrationEventPublisher;
    private readonly ILogger<TaskTrackerDbContext> _logger;

    public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options, IPublisher integrationEventPublisher, ILogger<TaskTrackerDbContext> logger) : base(options)
    {
        _integrationEventPublisher = integrationEventPublisher;
        _logger = logger;
    }

    public DbSet<Domain.Task> Tasks { get; set; }
    public DbSet<Popug> Popugs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Task

        modelBuilder.Entity<Domain.Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title);
            entity.Property(e => e.Description);
            entity.Property(e => e.Status).HasConversion(v => v.ToString(), v => (Domain.TaskStatus)Enum.Parse(typeof(Domain.TaskStatus), v));
            entity.OwnsOne<TaskPrice>(e => e.Price, entity =>
            {
                entity.Property(e => e.Fee);
                entity.Property(e => e.Reward);
            } );
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Assignment

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<Popug>(e => e.CurrentPopug).WithMany();
            entity.HasOne<Popug>(e => e.NewPopug).WithMany();
            entity.Property(e => e.AssignedAt);
            entity.Ignore(e => e.DomainEvents);
        });
        
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
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
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