using Confluent.Kafka;

namespace Auth.Integration;

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

        _logger.LogInformation("Sending {Message} to {Topic}..", topic, message);

        await _producer.ProduceAsync(topic, new Message<Null, string>
{
            Value = message
        }, cancellationToken);
        
        _logger.LogInformation("Message {Message} to {Topic} sent", topic, message);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}