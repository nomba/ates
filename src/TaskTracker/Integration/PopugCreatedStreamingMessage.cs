using MediatR;
using TaskTracker.Domain;

namespace TaskTracker.Integration;

public class PopugCreatedStreamingMessage : INotification
{
    public Guid Id { get; set; }
    public string Username { get; }
    public string FullName { get; set; }
    public RoleType Role { get;  set; }
    public string? Email { get; init; }
    public bool IsActive { get;  set; }
}