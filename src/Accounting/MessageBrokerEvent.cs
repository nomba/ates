using MediatR;

namespace Accounting;

public class MessageBrokerEvent<T> : INotification
{
    public MessageBrokerEvent(T data, Guid id, MessageBrokerEventName name, int version, DateTime createdAt)
    {
        Data = data;
        Id = id;
        Name = name;
        Version = version;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }
    public MessageBrokerEventName Name { get; }
    public int Version { get; }
    public DateTime CreatedAt { get; }
    public T Data { get; }
}

public class MessageBrokerEvent : INotification
{
    public MessageBrokerEvent(Guid id, MessageBrokerEventName name, MessageBrokerEventType type, int version, DateTime createdAt, object data)
    {
        Id = id;
        Name = name;
        Version = version;
        Type = type;
        CreatedAt = createdAt;
        Data = data;
    }

    public Guid Id { get; }
    public MessageBrokerEventName Name { get; }
    public MessageBrokerEventType Type { get; }
    public int Version { get; }
    public DateTime CreatedAt { get; }
    public object Data { get; }
}