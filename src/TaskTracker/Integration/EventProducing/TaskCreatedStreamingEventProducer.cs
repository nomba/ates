using TaskTracker.Domain.Events;

namespace TaskTracker.Integration.EventProducing;

internal class TaskCreatedStreamingEventProducer : MessageBrokerDomainEventHandler<TaskCreatedDomainEvent, TaskCreatedStreamingEvent>
{
    public TaskCreatedStreamingEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }

    protected override TaskCreatedStreamingEvent MapToMessageBrokerEvent(TaskCreatedDomainEvent domainEvent)
    {
        return new TaskCreatedStreamingEvent(Guid.NewGuid(), DateTime.UtcNow, new TaskCreatedStreamingEvent.DataType
        {
            Id = domainEvent.Task.Id,
            AssigneeId = domainEvent.Task.Assignee.Id,
            Title = domainEvent.Task.Title,
            Description = domainEvent.Task.Description,
            Fee = (double)domainEvent.Task.Price.Fee,
            Reward = (double)domainEvent.Task.Price.Reward,
            Status = (TaskStatus)domainEvent.Task.Status
        });
    }

    protected override string GetTopic() => "task-streaming";
}