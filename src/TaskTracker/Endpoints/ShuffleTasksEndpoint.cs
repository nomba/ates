using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskTracker.Domain;
using TaskStatus = TaskTracker.Domain.TaskStatus;

namespace TaskTracker.Endpoints;

public class ShuffleTasksEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult<ShuffleTasksResponse>
{
    private readonly TaskTrackerDbContext _dbContext;

    public ShuffleTasksEndpoint(TaskTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpPost("/tasks/shuffle")]
    [SwaggerOperation(Summary = "Shuffle all tasks")]
    public override async Task<ActionResult<ShuffleTasksResponse>> HandleAsync(CancellationToken cancellationToken = new())
    {
        // Get all incomplete tasks 
        var incompleteTasks = await _dbContext.Tasks.Where(t=> t.Status == TaskStatus.Submitted).ToListAsync(cancellationToken);
        
        // Get all regular popugs
        var regularPopugs = await _dbContext.Popugs.Where(p => p.Role == RoleType.Employee).ToListAsync(cancellationToken);

        // Count all popugs used in task shuffling
        var reassignedPopugs = new HashSet<Popug>();
        
        foreach (var task in incompleteTasks)
        {
            var rnd = new Random();
            var newPopug = task.Assignee;

            // Pick a new random popug
            while (newPopug == task.Assignee)
                newPopug = regularPopugs[rnd.Next(0, regularPopugs.Count - 1)];

            reassignedPopugs.Add(newPopug);
            task.Reassign(newPopug, DateTime.Now);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Ok(new ShuffleTasksResponse
        {
            ReassignedTaskCount = incompleteTasks.Count,
            ReassignedPopugCount = reassignedPopugs.Count
        });
    }
}