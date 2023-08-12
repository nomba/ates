using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskTracker.Endpoints;

public class ListTasksEndpoint : EndpointBaseAsync.WithRequest<ListTasksRequest>.WithActionResult<ListTasksResponse>
{
    private readonly TaskTrackerDbContext _dbContext;

    public ListTasksEndpoint(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [AllowAnonymous]
    [HttpGet("/tasks")]
    [SwaggerOperation(Summary = "List all tasks")]
    public override async Task<ActionResult<ListTasksResponse>> HandleAsync(ListTasksRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Auth
        
        var taskQuery = _dbContext.Tasks.AsQueryable();

        if (request.OnlyAssignee is { } assigneeId)
            taskQuery = taskQuery.Where(t => t.Assignee.Id == assigneeId);

        if (request.OnlyStatus is { } status)
            taskQuery = taskQuery.Where(t => t.Status == status);

        var tasks = await taskQuery.Select(t=> new ListTasksResponseItem
        {
            Title = t.Title,
            Description = t.Description,
            Fee = t.Price.Fee,
            Reward = t.Price.Reward,
            AssigneeId = t.Assignee.Id,
            Status = t.Status
        }).ToListAsync(cancellationToken: cancellationToken);

        return Ok(new ListTasksResponse { Items = tasks });
    }
}