using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Analytics.Endpoints;

public class GetTaskTopEndpoint : EndpointBaseAsync.WithRequest<GetTaskTopRequest>.WithActionResult<GetTaskTopResponse>
{
    private readonly AnalyticsDbContext _dbContext;

    public GetTaskTopEndpoint(AnalyticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [AllowAnonymous]
    [HttpGet("/task-top")]
    [SwaggerOperation(Summary = "Get task top")]
    public override async Task<ActionResult<GetTaskTopResponse>> HandleAsync([FromQuery] GetTaskTopRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Auth

        var topQuery = _dbContext.TaskTopItems.AsQueryable();

        if (request.DayFrom is { } dayFrom)
            topQuery = topQuery.Where(t => t.CalculatedAt >= dayFrom);

        if (request.DayTo is { } dayTo)
            topQuery = topQuery.Where(t => t.CalculatedAt <= dayTo);

        var topItems = await topQuery.ToListAsync(cancellationToken);

        return Ok(new GetTaskTopResponse
        {
            TopItems = topItems.Select(t => t.ToShortDescription()).ToList()
        });
    }
}