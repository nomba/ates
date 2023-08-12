namespace TaskTracker.Domain;

public class TaskPrice
{
    public decimal Fee { get; }
    public decimal Reward { get; }

    private TaskPrice()
    {
        // For EF
    }

    // TODO: Implement price calculating
    public static TaskPrice RollDice => new();
}