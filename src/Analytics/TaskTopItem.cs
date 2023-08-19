namespace Analytics;

public class TaskTopItem
{
    public int Id { get; set; }
    public string Task { get; set; }
    public TaskTopType TopType { get; set; }
    public DateTime CalculatedAt { get; set; }
}