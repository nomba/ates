using TaskTracker.Domain;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Integration.EventConsuming;

internal class PopugCreatedStreamingEventConsumer : MediatrBasedMessageBrokerEventConsumer<PopugCreatedStreamingEvent>
{
    private readonly TaskTrackerDbContext _dbContext;

    public PopugCreatedStreamingEventConsumer(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Consume(PopugCreatedStreamingEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not PopugCreatedStreamingEvent.DataType eventData)
            throw new InvalidCastException();

        _dbContext.Popugs.Add(new Popug(eventData.Username, eventData.FullName, eventData.Role));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}