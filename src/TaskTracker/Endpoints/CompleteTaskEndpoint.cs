using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskTracker.Endpoints;

public class CompleteTaskEndpoint : EndpointBaseAsync.WithRequest<CompleteTaskRequest>.WithActionResult
{
    private readonly TaskTrackerDbContext _dbContext;

    public CompleteTaskEndpoint(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpPost("/tasks/{TaskId}/complete")]
    [SwaggerOperation(Summary = "Complete specific task")]
    public override async Task<ActionResult> HandleAsync([FromRoute] CompleteTaskRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        // TODO: Auth

        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        
        if (task is null)
            return NotFound();
        
        task.Complete();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}