namespace Analytics.Domain;

public class Task : Entity
{
    public Popug Assignee { get; set; }
    public string Title { get; set; }
    public double Fee { get; set; }
    public double Reward { get; set; }
}