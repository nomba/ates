namespace TaskTracker.Integration;

internal class ResilientKafkaMessageHandler : IKafkaMessageHandler
{
    private readonly IKafkaMessageHandler _kafkaMessageHandler;
    private readonly ILogger<ResilientKafkaMessageHandler> _logger;

    public ResilientKafkaMessageHandler(IKafkaMessageHandler kafkaMessageHandler, ILogger<ResilientKafkaMessageHandler> logger)
    {
        _kafkaMessageHandler = kafkaMessageHandler;
        _logger = logger;
    }

    public async Task Handle(string topic, string message)
    {
        try
        {
            await _kafkaMessageHandler.Handle(topic, message);
        }
        catch (Exception ex)
        {
            // TODO: Ideally here must be dealing with dead letter queue (DLQ)
            
            // Currently just log dead message
            _logger.LogError(ex, "Can't handle message from topic [{Topic}]: {MessageBody}", topic, message);
        }
    }
}