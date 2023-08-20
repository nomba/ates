namespace TaskTracker.Integration;

internal interface IKafkaMessageProducer : IDisposable
{
    Task Produce(string topic, string message, CancellationToken cancellationToken);
}