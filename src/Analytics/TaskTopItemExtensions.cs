namespace Analytics;

public static class TaskTopItemExtensions
{
    // Format from requirements: `03.03 — самая дорогая задача — 28$` 
    public static string ToShortDescription(this TaskTopItem taskTopItem)
    {
        return $"{taskTopItem.TopType} - {taskTopItem.CalculatedAt.Date} – {taskTopItem.Task}";
    }
}