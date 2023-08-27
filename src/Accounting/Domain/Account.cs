using Accounting.Domain.Events;

namespace Accounting.Domain;

public class Account : Entity
{
    private readonly List<Transaction> _transactions = new();

    public Account(Popug owner)
    {
        Owner = owner;
    }

    public Popug Owner { get; }
    public double Balance { get; private set; }
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    
    public void Deposit(double amount, string description)
    {
        if (amount <= 0)
            throw new ArgumentException(nameof(amount));
            
        var deposit = new Transaction(description, 0, amount);
        _transactions.Add(deposit);
        
        Balance += amount;
        _domainEvents.Add(new TransactionCommittedDomainEvent(this, deposit));
    }
    
    public void Withdraw(double amount, string description)
    {
        if (amount <= 0)
            throw new ArgumentException(nameof(amount));
        
        var withdrawal = new Transaction(description, amount, 0);
        _transactions.Add(withdrawal);
        
        Balance -= amount;
        _domainEvents.Add(new TransactionCommittedDomainEvent(this, withdrawal));
    }
}