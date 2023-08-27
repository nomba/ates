namespace Accounting;

public struct MessageBrokerEventName
{
    public MessageBrokerEventName(string domain, string value)
    {
        Domain = domain;
        Value = value;
    }

    public string Value { get; }
    public string Domain { get; }
}