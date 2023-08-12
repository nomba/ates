using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskTracker.Endpoints;

public class GetTaskByIdEndpoint : EndpointBaseAsync.WithRequest<GetTaskByIdRequest>.WithActionResult<GetTaskByIdResponse>
{
    private readonly TaskTrackerDbContext _dbContext;

    public GetTaskByIdEndpoint(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [AllowAnonymous]
    [HttpGet("/tasks/{TaskId}")]
    [SwaggerOperation(Summary = "Get full info for specific task")]
    public override async Task<ActionResult<GetTaskByIdResponse>> HandleAsync([FromRoute] GetTaskByIdRequest request, CancellationToken cancellationToken = new())
    {
        // TODO: Auth

        var task = await _dbContext.Tasks
            .Include(t => t.Assignee)
            .Include(t => t.Assignments)
                .ThenInclude(a=> a.NewPopug)
            .Include(t => t.Assignments)
                .ThenInclude(a=> a.CurrentPopug)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        
        if (task is null)
            return NotFound();

        return Ok(new GetTaskByIdResponse
        {
            Title = task.Title,
            Description = task.Description,
            Fee = task.Price.Fee,
            Reward = task.Price.Reward,
            AssigneeId = task.Assignee.Id,
            Status = task.Status,
            Assignments = task.Assignments.Select(a => new AssignmentDto
            {
                CurrentPopugId = a.CurrentPopug?.Id,
                NewPopugId = a.NewPopug.Id,
                AssignedAt = a.AssignedAt
            }).ToList()
        });
    }
}