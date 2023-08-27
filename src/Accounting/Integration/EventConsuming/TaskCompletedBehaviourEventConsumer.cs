using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Accounting.Integration.EventConsuming;

internal class TaskCompletedBehaviourEventConsumer : MediatrBasedMessageBrokerEventConsumer<TaskCompletedBehaviourEvent>
{
    private readonly AccountingDbContext _dbContext;

    public TaskCompletedBehaviourEventConsumer(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task Consume(TaskCompletedBehaviourEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not TaskCompletedBehaviourEvent.DataType eventData)
            throw new InvalidCastException();

        // Find account for popug
        
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Owner.Id == eventData.AssigneeId, cancellationToken);
        
        if (account is null)
            throw new InvalidOperationException("Account not found");
        
        // Get task price
        
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == eventData.Id, cancellationToken);
        
        if (task is null)
            throw new InvalidOperationException("Task not found");

        // Give popug reward
        
        account.Deposit(task.Reward, $"Reward for {eventData.Id}");
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}