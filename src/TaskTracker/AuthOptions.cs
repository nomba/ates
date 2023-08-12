namespace TaskTracker;

public class AuthOptions
{
    public const string JWT_SECRET_KEY = "THE_SECRET_EVERY_POPUG_KNOW";
    public const string ISSUER = "ATES TaskTracker";
    public const string AUDIENCE  = "ATES TaskTracker Client";
    
    public string JwtSecretKey { get; set; } = JWT_SECRET_KEY;
}