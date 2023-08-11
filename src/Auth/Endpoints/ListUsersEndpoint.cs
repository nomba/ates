using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth.Endpoints;

public class ListUsersEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult<ListUsersResponse>
{
    private readonly AuthDbContext _authDbContext;

    public ListUsersEndpoint(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }
    
    [Authorize]
    [HttpGet("/users")]
    [SwaggerOperation(Summary = "List all popugs")]
    public override async Task<ActionResult<ListUsersResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // TODO: Authorize manually
        //
        // var claims = User.Claims;
        // var authHeaders = HttpContext.Request.Headers.Authorization;
        //
        //
        // return Unauthorized();
        // // return Task.FromResult<IReadOnlyCollection<string>>(new[] { $"{User.Identity.Name}"});

        var popugs  = await _authDbContext.Pogugs.ToListAsync(cancellationToken);
        
        return Ok(new ListUsersResponse
        {
            Items = popugs.Select(p => new ListUsersResponseItem
            {
                Pid = p.Id,
                Username = p.Username,
                FullName = p.FullName,
                Role = p.Role,
                Email = p.Email,
                IsActive = p.IsActive
            }).ToList()
        });
    }
}