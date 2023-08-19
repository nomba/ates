namespace Analytics.Endpoints;

public class GetMoneyStatsResponse
{
    public IReadOnlyCollection<MoneyDailyStatsItem> DailyStats { get; set; }
}

public class MoneyDailyStatsItem
{
    public DateTime Day { get; set; }
    public double ManagementTotalDailyIncome { get; set; } 
    public int PopugDailyLosersCount { get; set; } 
}

