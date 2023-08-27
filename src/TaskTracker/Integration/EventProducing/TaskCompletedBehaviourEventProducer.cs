using TaskTracker.Domain.Events;

namespace TaskTracker.Integration.EventProducing;

internal class TaskCompletedBehaviourEventProducer : MessageBrokerDomainEventHandler<TaskCompletedDomainEvent, TaskCompletedBehaviourEvent>
{
    public TaskCompletedBehaviourEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }

    protected override TaskCompletedBehaviourEvent MapToMessageBrokerEvent(TaskCompletedDomainEvent domainEvent)
    {
        return new TaskCompletedBehaviourEvent(Guid.NewGuid(), DateTime.UtcNow, new TaskCompletedBehaviourEvent.DataType
        {
            Id = domainEvent.Task.Id,
            AssigneeId = domainEvent.Task.Assignee.Id
        });
    }

    protected override string GetTopic() => "task-life-cycle";
}