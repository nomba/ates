namespace Accounting.Domain;

public class Task : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Fee { get; set; }
    public double Reward { get; set; }
}