using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth.Endpoints;

public class CreateTokenEndpoint: EndpointBaseAsync
    .WithRequest<AuthenticateRequest>
    .WithActionResult<string>
{
    [HttpPost("/token")]
    [SwaggerOperation(Summary = "Create access token to API")]
    public override async Task<ActionResult<string>> HandleAsync(AuthenticateRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}