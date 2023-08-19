using Analytics.Domain;

namespace Analytics;

public class MoneyStatsItem
{
    public int Id { get; set; }
    public Guid PopugId { get; set; }
    public string PopugFullName { get; set; }
    public RoleType PopugRole { get; set; }
    public double CurrentBalance { get; set; }
    public DateTime CalculatedAt { get; set; }
}