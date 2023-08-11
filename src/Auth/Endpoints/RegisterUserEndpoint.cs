using Ardalis.ApiEndpoints;
using Auth.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth.Endpoints;

public class RegisterUserEndpoint : EndpointBaseAsync.WithRequest<RegisterUserRequest>.WithActionResult<RegisterUserResponse>
{
    private readonly AuthDbContext _authDbContext;

    public RegisterUserEndpoint(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }
    
    [Authorize]
    [HttpPost("/users")]
    [SwaggerOperation(Summary = "Register a new popug")]
    public override async Task<ActionResult<RegisterUserResponse>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        // TODO: Authorize manually
        
        
        // Add a new popug
        var popug = new Popug(request.Username, request.FullName, request.Role) { Email = request.Email };
        _authDbContext.Pogugs.Add(popug);
        
        await _authDbContext.SaveChangesAsync(cancellationToken);
        return Created($"/users/{popug.Id}",new RegisterUserResponse { PublicId = popug.Id, Username = popug.Username, FullName = popug.FullName, Email = popug.Email });
    }
}