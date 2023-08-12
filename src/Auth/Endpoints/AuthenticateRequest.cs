namespace Auth.Endpoints;

public class AuthenticateRequest
{
    // Popugs remembers only their own name (no password)
    public string Username { get; set; }
}