using TaskTracker.Domain.Events;

namespace TaskTracker.Integration.EventProducing;

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
            AssigneeId = domainEvent.Task.Assignee.Id
        });
    }

    protected override string GetTopic() => "task-life-cycle";
}