namespace SchemaRegistry;

public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string? FailureMessage { get; private set; }

    public static ValidationResult Success { get; } = new() {IsValid = true};
    public static ValidationResult Failure(string message) => new() {IsValid = true, FailureMessage = message};
}