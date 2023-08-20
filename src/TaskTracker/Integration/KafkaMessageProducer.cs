using Confluent.Kafka;

namespace TaskTracker.Integration;

internal class KafkaMessageProducer : IKafkaMessageProducer
{
    private readonly ILogger<KafkaMessageProducer> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaMessageProducer(ILogger<KafkaMessageProducer> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task Produce(string topic, string message, CancellationToken cancellationToken)
    {
        if (topic == null) throw new ArgumentNullException(nameof(topic));
        if (message == null) throw new ArgumentNullException(nameof(message));

        var messageHash = message.GetHashCode();
        _logger.LogInformation("Sending {Message} to {Topic} [{MessageHash}]..", topic, message, messageHash);

        await _producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = message
        }, cancellationToken);

        _logger.LogInformation("Message [{MessageHash}] to {Topic} sent", messageHash, topic);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}