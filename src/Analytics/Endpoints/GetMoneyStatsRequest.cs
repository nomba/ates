namespace Analytics.Endpoints;

public class GetMoneyStatsRequest
{
    public DateTime? DayFrom { get; set; }
    public DateTime? DayTo { get; set; }
}