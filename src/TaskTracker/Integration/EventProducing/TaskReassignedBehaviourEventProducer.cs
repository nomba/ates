using TaskTracker.Domain.Events;

namespace TaskTracker.Integration.EventProducing;

internal class TaskReassignedBehaviourEventProducer : MessageBrokerDomainEventHandler<TaskReassignedDomainEvent, TaskReassignedBehaviourEvent>
{
    public TaskReassignedBehaviourEventProducer(IKafkaMessageProducer kafkaMessageProducer) : base(kafkaMessageProducer)
    {
    }

    protected override TaskReassignedBehaviourEvent MapToMessageBrokerEvent(TaskReassignedDomainEvent domainEvent)
    {
        var lastAssigment = domainEvent.Task.Assignments.Last();

        return new TaskReassignedBehaviourEvent(Guid.NewGuid(), DateTime.UtcNow, new TaskReassignedBehaviourEvent.DataType()
        {
            Id = domainEvent.Task.Id,
            NewAssigneeId = lastAssigment.NewPopug.Id
        });
    }

    protected override string GetTopic() => "task-life-cycle";
}