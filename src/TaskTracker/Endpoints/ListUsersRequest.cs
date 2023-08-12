using TaskTracker.Domain;

namespace TaskTracker.Endpoints;

public class ListUsersRequest
{
    public RoleType? OnlyRole { get; set; }
}