namespace Analytics.Domain;

public class Account : Entity
{
    public double Balance { get; set; }
    public Popug Owner { get; set; }
}