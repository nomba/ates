using Analytics.Domain;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Analytics.Endpoints;

public class GetMoneyStatsEndpoint : EndpointBaseAsync.WithRequest<GetMoneyStatsRequest>.WithActionResult<GetMoneyStatsResponse>
{
    private readonly AnalyticsDbContext _dbContext;

    public GetMoneyStatsEndpoint(AnalyticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpGet("/money-stats")]
    [SwaggerOperation(Summary = "Get money statistics")]
    public override async Task<ActionResult<GetMoneyStatsResponse>> HandleAsync([FromQuery]GetMoneyStatsRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Auth

        var moneyQuery = _dbContext.MoneyStatsItems.AsQueryable();

        if (request.DayFrom is { } dayFrom)
            moneyQuery = moneyQuery.Where(t => t.CalculatedAt >= dayFrom);

        if (request.DayTo is { } dayTo)
            moneyQuery = moneyQuery.Where(t => t.CalculatedAt <= dayTo);

        var dailyGroups = await moneyQuery
            .GroupBy(item=> item.CalculatedAt.Date).ToListAsync(cancellationToken);
        
        var dailyStatsItems = dailyGroups.Select(dailyGroup => new MoneyDailyStatsItem
        {
            Day = dailyGroup.Key,
            PopugDailyLosersCount = dailyGroup.DistinctBy(item=> item.PopugFullName).Count(item => item is {PopugRole: RoleType.Employee, CurrentBalance: < 0}),
            ManagementTotalDailyIncome = dailyGroup.DistinctBy(item=> item.PopugFullName).Where(item => item.PopugRole == RoleType.Manager).Sum(item => item.CurrentBalance)
        });
        
        return Ok(new GetMoneyStatsResponse
        {
            DailyStats = dailyStatsItems.ToList()
        });
    }
}