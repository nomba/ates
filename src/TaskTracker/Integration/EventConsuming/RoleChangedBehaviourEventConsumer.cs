using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Integration.EventConsuming;

internal class RoleChangedBehaviourEventConsumer : MediatrBasedMessageBrokerEventConsumer<RoleChangedBehaviourEvent>
{
    private readonly TaskTrackerDbContext _dbContext;

    public RoleChangedBehaviourEventConsumer(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task Consume(RoleChangedBehaviourEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not RoleChangedBehaviourEvent.DataType eventData)
            throw new InvalidCastException();

        // TODO: Handle variant when popug role changed event received before popug created event

        var popug = await _dbContext.Popugs.FirstOrDefaultAsync(p => p.Id == eventData.PopugId, cancellationToken);

        if (popug is null)
            throw new InvalidOperationException("Popug not found");
        
        popug.Role = eventData.NewRole;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}