using System.Text.Json.Nodes;
using Json.Schema;

namespace SchemaRegistry;

public static class Validator
{
    public static ValidationResult ValidateEvent(string @event, string domainName, string eventName, int eventVersion)
    {
        var schemaFilePath = GetSchemaFilePath(domainName, eventName, eventVersion);

        if (!File.Exists(schemaFilePath))
            throw new InvalidOperationException($"Schema file does not exist");

        var schema = JsonSchema.FromFile(schemaFilePath);
        var evaluationResults = schema.Evaluate(JsonNode.Parse(@event));

        return evaluationResults is {IsValid: true, HasErrors: false}
            ? ValidationResult.Success
            : ValidationResult.Failure(string.Join("; ", evaluationResults.Errors?.Values ?? ArraySegment<string>.Empty));
    }

    private static string GetSchemaFilePath(string domainName, string eventName, int eventVersion)
    {
        if (string.IsNullOrWhiteSpace(domainName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(domainName));

        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(eventName));

        if (eventVersion <= 0)
            throw new ArgumentException("Event version must be positive integer.");

        var currentProcess = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
        if (currentProcess is null || Path.GetDirectoryName(currentProcess) is not {} currentDir)
            throw new InvalidOperationException("Unable to get define current directory.");

        return Path.Combine(currentDir, "Schemas", domainName, eventName, $"{eventVersion.ToString()}.json");
    }
}