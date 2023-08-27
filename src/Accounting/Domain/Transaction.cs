namespace Accounting.Domain;

public class Transaction : Entity
{
    public Transaction(string description, double credit, double debit)
    {
        Description = description;
        Credit = credit;
        Debit = debit;
    }

    public string Description { get; }
    public double Credit { get; }
    public double Debit { get; }
}