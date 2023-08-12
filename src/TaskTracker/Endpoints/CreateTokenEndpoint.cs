using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskTracker.Endpoints;

public class CreateTokenEndpoint : EndpointBaseAsync
    .WithRequest<AuthenticateRequest>
    .WithActionResult<string>
{
    private readonly TaskTrackerDbContext _dbContext;
    private readonly AuthOptions _authOptions;

    public CreateTokenEndpoint(TaskTrackerDbContext dbContext, IOptions<AuthOptions> options)
    {
        _dbContext = dbContext;
        _authOptions = options.Value;
    }

    [HttpPost("/token")]
    [SwaggerOperation(Summary = "Create access token to API")]
    public override async Task<ActionResult<string>> HandleAsync(AuthenticateRequest request, CancellationToken cancellationToken = new())
    {
        var identity = await GetIdentity(request.Username);

        if (identity is null)
            return NotFound($"Popug \"{request.Username}\" not found");

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

    // Popug can not remember password, it's pretty hard for him/her, therefore identify only by username
    private async Task<ClaimsIdentity?> GetIdentity(string username)
    {
        var popug = await _dbContext.Popugs.FirstOrDefaultAsync(p => p.Username == username);

        if (popug is null)
            return default;

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, username),
            new("popug_pid", popug.Id.ToString()),
        };

        return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, default);
    }
}