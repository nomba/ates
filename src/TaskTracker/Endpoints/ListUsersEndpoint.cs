using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskTracker.Endpoints;

public class ListUsersEndpoint : EndpointBaseAsync.WithRequest<ListUsersRequest>.WithActionResult<ListUsersResponse>
{
    private readonly TaskTrackerDbContext _dbContext;

    public ListUsersEndpoint(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize]
    [AllowAnonymous]
    // ??? For Task Tracker context only needs request regular employee to create a new task. Can limit the endpoint and implement only `/employees`
    [HttpGet("/users")]
    [SwaggerOperation(Summary = "List popugs")]
    public override async Task<ActionResult<ListUsersResponse>> HandleAsync(ListUsersRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Auth
        
        var popugQuery = _dbContext.Popugs.AsQueryable();

        if (request.OnlyRole is { } role)
            popugQuery = popugQuery.Where(p => p.Role == role);

        var popugs = await popugQuery.Select(p =>
            new ListUsersResponseItem
            {
                Pid = p.Id,
                Username = p.Username,
                FullName = p.FullName,
                Role = p.Role,
                Email = p.Email,
                IsActive = p.IsActive
            }
        ).ToListAsync(cancellationToken);

        return Ok(new ListUsersResponse { Items = popugs });
    }
}