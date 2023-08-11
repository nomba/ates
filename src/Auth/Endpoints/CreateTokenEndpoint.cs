using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth.Endpoints;

public class CreateTokenEndpoint : EndpointBaseAsync
    .WithRequest<AuthenticateRequest>
    .WithActionResult<string>
{
    private readonly AuthDbContext _authDbContext;
    private readonly AuthOptions _authOptions;

    public CreateTokenEndpoint(AuthDbContext authDbContext, IOptions<AuthOptions> options)
    {
        _authDbContext = authDbContext;
        _authOptions = options.Value;
    }

    [HttpPost("/token")]
    [SwaggerOperation(Summary = "Create access token to API")]
    public override async Task<ActionResult<string>> HandleAsync(AuthenticateRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var identity = GetIdentity(request.Username);

        if (identity is null)
            return NotFound();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.JwtSecretKey);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

        var encodedJwt = tokenHandler.WriteToken(jwt);
        return encodedJwt;
    }

    private static ClaimsIdentity? GetIdentity(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, username),
            new("popug_pid", Guid.NewGuid().ToString()),
        };
        
        return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, default);
    }
}