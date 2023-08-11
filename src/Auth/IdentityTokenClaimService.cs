using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth;

public class IdentityTokenClaimService : ITokenClaimsService
{
    private readonly AuthOptions _options;

    public IdentityTokenClaimService(IOptions<AuthOptions> options)
    {
        _options = options.Value;
    }
    
    public Task<string> GetTokenAsync(string userName)
    {
        // var tokenHandler = new JwtSecurityTokenHandler();
        //
        // var jwt = new JwtSecurityToken(
        //     issuer: AuthOptions.ISSUER,
        //     audience: AuthOptions.AUDIENCE,
        //     notBefore: now,
        //     claims: identity.Claims,
        //     expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
        //     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        //
        // // tokenHandler.CreateToken()

        return Task.FromResult(string.Empty);

    }
}