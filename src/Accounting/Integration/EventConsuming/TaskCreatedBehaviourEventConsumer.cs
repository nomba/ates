using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Accounting.Integration.EventConsuming;

internal class TaskCreatedBehaviourEventConsumer : MediatrBasedMessageBrokerEventConsumer<TaskCreatedBehaviourEvent>
{
    private readonly AccountingDbContext _dbContext;

    public TaskCreatedBehaviourEventConsumer(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // TODO: Introduce Commands layer because withdrawal is duplicated in TaskReassignedHandler  
    protected override async Task Consume(TaskCreatedBehaviourEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Data is not TaskCreatedBehaviourEvent.DataType eventData)
            throw new InvalidCastException();

        // Find account for popug

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Owner.Id == eventData.AssigneeId, cancellationToken);

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