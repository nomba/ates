namespace Auth;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}