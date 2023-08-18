namespace Analytics;

internal class SeedingOptions
{
    // `supo` means SUper POpug
    private const string DEFAULT_SYSTEM_POPUG = "supo";
    
    public bool RecreateDatabase { get; set; }
    public string SuperPopug { get; set; } = DEFAULT_SYSTEM_POPUG;
}