namespace TaskTracker.Integration;

internal interface IKafkaMessageHandler
{
    Task Handle(string topic, string message);
}