namespace TaskTracker.Domain;

public class TaskPrice
{
    public decimal Fee { get; private set; }
    public decimal Reward { get; private set; }

    private TaskPrice()
    {
        // For EF
        
      
    }
    
    // Implement requirements `docs/requirements.md`
    // ```
    // - формула, которая говорит сколько списать денег с сотрудника при ассайне задачи — `rand(-10..-20)$`
    // - формула, которая говорит сколько начислить денег сотруднику для выполненой задачи — `rand(20..40)$`
    // ```
    public static TaskPrice RollDice()
    {
        var rnd = new Random();

        return new TaskPrice
        {
            Fee = rnd.Next(10, 20),
            Reward = rnd.Next(20, 40)
        };
    }
}