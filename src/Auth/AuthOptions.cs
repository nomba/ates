namespace Auth;

public class AuthOptions
{
    public const string JWT_SECRET_KEY = "THE_SECRET_EVERY_POPUG_KNOW";
    public const string ISSUER = "ATES Auth";
    public const string AUDIENCE  = "ATES Auth Client";
    
    public string JwtSecretKey { get; set; } = JWT_SECRET_KEY;
}