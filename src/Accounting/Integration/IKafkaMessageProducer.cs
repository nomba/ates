namespace Accounting.Integration;

internal interface IKafkaMessageProducer : IDisposable
{
    Task Produce(string topic, string message, CancellationToken cancellationToken);
}

internal class ResistantKafkaMessageProducer : IKafkaMessageProducer
{
    private readonly IKafkaMessageProducer _kafkaMessageProducer;
    private readonly ILogger<ResistantKafkaMessageProducer> _logger;

    public ResistantKafkaMessageProducer(IKafkaMessageProducer kafkaMessageProducer, ILogger<ResistantKafkaMessageProducer> logger)
    {
        _kafkaMessageProducer = kafkaMessageProducer;
        _logger = logger;
    }
    
    public void Dispose()
    {
        _kafkaMessageProducer.Dispose();
    }

    public async Task Produce(string topic, string message, CancellationToken cancellationToken)
    {
        // TODO: Use Poly for retry policy
        
        var messageHash = message.GetHashCode();
        _logger.LogInformation("Sending {Message} to {Topic} [{MessageHash}]..", topic, message, messageHash);

        await _kafkaMessageProducer.Produce(topic, message, cancellationToken);   
    
        _logger.LogInformation("Message [{MessageHash}] to {Topic} sent", messageHash, topic);
    }
}