namespace Accounting;

public class AccountingOptions
{
    public const string JWT_SECRET_KEY = "THE_SECRET_EVERY_POPUG_KNOW";
    public const string ISSUER = "ATES Analytics";
    public const string AUDIENCE  = "ATES Analytics Client";
    
    public string JwtSecretKey { get; set; } = JWT_SECRET_KEY;
}