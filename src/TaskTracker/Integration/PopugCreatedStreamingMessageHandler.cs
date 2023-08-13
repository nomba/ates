using MediatR;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Integration;

internal class PopugCreatedStreamingMessageHandler : INotificationHandler<PopugCreatedStreamingMessage>
{
    private readonly TaskTrackerDbContext _dbContext;

    public PopugCreatedStreamingMessageHandler(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task Handle(PopugCreatedStreamingMessage notification, CancellationToken cancellationToken)
    {
       // Save in Database
       throw new NotImplementedException();
    }
}