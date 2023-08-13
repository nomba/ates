using MediatR;

namespace TaskTracker.Integration;

internal class KafkaMessageHandler : IKafkaMessageHandler
{
    private readonly IPublisher _publisher;

    public KafkaMessageHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(string topic, string message)
    {
        // TODO: Fix hardcoded
        
        await _publisher.Publish(new PopugCreatedStreamingMessage()
        {
            
        });
            
        if (topic.Contains("-streaming"))
        {
            // Streaming 
        }
        
    }
}