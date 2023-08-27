using Confluent.Kafka;

namespace TaskTracker.Integration;

internal class KafkaMessageListener : BackgroundService
{
    private readonly IKafkaMessageHandler _kafkaMessageHandler;
    private readonly ILogger<KafkaMessageListener> _logger;
    private readonly ConsumerConfig _config;

    public KafkaMessageListener(IKafkaMessageHandler kafkaMessageHandler, ILogger<KafkaMessageListener> logger)
    {
        _kafkaMessageHandler = kafkaMessageHandler;
        _logger = logger;
        _config = new ConsumerConfig
        {
            GroupId = "ates-task-tracker",
            BootstrapServers = "localhost:9092",

            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        consumer.Subscribe(new[] {"user-life-cycle", "user-streaming"});

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // ReSharper disable once AccessToDisposedClosure
                var consumingTask = Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
                var consumeResult = await consumingTask;

                await _kafkaMessageHandler.Handle(consumeResult.Topic, consumeResult.Message.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consuming message failed");
            }
        }

        consumer.Close();
    }
}