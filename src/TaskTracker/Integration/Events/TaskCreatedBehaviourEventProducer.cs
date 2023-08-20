using TaskTracker.Domain.Events;

namespace TaskTracker.Integration.Events;

internal class TaskCreatedBehaviourEventProducer : MessageBrokerDomainEventHandler<TaskCreatedDomainEvent, TaskCreatedBehaviourEvent>
{
    public TaskCreatedBehaviourEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }

    protected override TaskCreatedBehaviourEvent MapToMessageBrokerEvent(TaskCreatedDomainEvent domainEvent)
    {
        return new TaskCreatedBehaviourEvent(Guid.NewGuid(), DateTime.UtcNow, new TaskCreatedBehaviourEvent.DataType
        {
            Id = domainEvent.Task.Id,
            Title = domainEvent.Task.Title,
            Description = domainEvent.Task.Description,
            Fee = (double) domainEvent.Task.Price.Fee,
            Reward = (double) domainEvent.Task.Price.Reward,
            Status = domainEvent.Task.Status.ToString()
        });
    }

    protected override string GetTopic() => "task-life-cycle";
}