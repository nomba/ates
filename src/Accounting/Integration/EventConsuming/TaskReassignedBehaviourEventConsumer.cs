using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Accounting.Integration.EventConsuming;

internal class TaskReassignedBehaviourEventConsumer : MediatrBasedMessageBrokerEventConsumer<TaskReassignedBehaviourEvent>
{
    private readonly AccountingDbContext _dbContext;

    public TaskReassignedBehaviourEventConsumer(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task Consume(TaskReassignedBehaviourEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not TaskReassignedBehaviourEvent.DataType eventData)
            throw new InvalidCastException();

        // Find account for popug

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Owner.Id == eventData.NewAssigneeId, cancellationToken);

        if (account is null)
            throw new InvalidOperationException("Account not found");

        // Get task price

        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == eventData.Id, cancellationToken);

        if (task is null)
            throw new InvalidOperationException("Task not found");

        // Take fee from popug

        account.Withdraw(task.Fee, $"Fee for {eventData.Id} assigned");
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}