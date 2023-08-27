using Accounting.Domain;
using Task = System.Threading.Tasks.Task;

namespace Accounting.Integration.EventConsuming;

internal class PopugCreatedStreamingEventConsumer : MediatrBasedMessageBrokerEventConsumer<PopugCreatedStreamingEvent>
{
    private readonly AccountingDbContext _dbContext;

    public PopugCreatedStreamingEventConsumer(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Consume(PopugCreatedStreamingEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not PopugCreatedStreamingEvent.DataType eventData)
            throw new InvalidCastException();

        _dbContext.Pogugs.Add(new Popug(eventData.Username, eventData.FullName, eventData.Role));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}