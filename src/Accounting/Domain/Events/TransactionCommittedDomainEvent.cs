namespace Accounting.Domain.Events;

internal class TransactionCommittedDomainEvent : DomainEvent
{
    public TransactionCommittedDomainEvent(Account account, Transaction transaction)
    {
        Account = account;
        Transaction = transaction;
    }
    
    public Account Account { get; }
    public Transaction Transaction { get; }
}