using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskTracker.Domain;

namespace TaskTracker.Endpoints;

public class CreateTaskEndpoint : EndpointBaseAsync.WithRequest<CreateTaskRequest>.WithActionResult<CreateTaskResponse>
{
    private readonly TaskTrackerDbContext _taskTrackerDbContext;

    public CreateTaskEndpoint(TaskTrackerDbContext taskTrackerDbContext)
    {
        _taskTrackerDbContext = taskTrackerDbContext;
    }

    [Authorize]
    [AllowAnonymous]
    [HttpPost("/tasks")]
    [SwaggerOperation(Summary = "Create a new task")]
    public override async Task<ActionResult<CreateTaskResponse>> HandleAsync(CreateTaskRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Authorize
        
        var taskAssignee = await _taskTrackerDbContext.Popugs.FindAsync(new object?[] { request.AssigneeId }, cancellationToken);
        if (taskAssignee is null)
            return NotFound($"Assignee popug \"{request.AssigneeId}\" not found");
        
        var task = new Domain.Task(request.Title, request.Description, taskAssignee, TaskPrice.RollDice, DateTime.Now);
        _taskTrackerDbContext.Tasks.Add(task);
        
        await _taskTrackerDbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateTaskResponse
        {
            PublicId = task.Id,
            AssigneeId = task.Assignee.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status
        };

        return Created($"/tasks/{task.Id}", response);
    }
}

public interface ITaskAssigneeFinder
{
    Task<Popug> Find(CancellationToken cancellationToken);
}

class TaskAssigneeFinder : ITaskAssigneeFinder
{
    private readonly TaskTrackerDbContext _taskTrackerDbContext;

    public TaskAssigneeFinder(TaskTrackerDbContext taskTrackerDbContext)
    {
        _taskTrackerDbContext = taskTrackerDbContext;
    }

    public Task<Popug> Find(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}