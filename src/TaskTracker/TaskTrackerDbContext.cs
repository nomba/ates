using Microsoft.EntityFrameworkCore;

namespace TaskTracker;

public class TaskTrackerDbContext : DbContext
{
    public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options)
    {
    }
}