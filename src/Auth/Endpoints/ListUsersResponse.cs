namespace Auth.Endpoints;

public class ListUsersResponse
{
    public IReadOnlyCollection<ListUsersResponseItem> Items { get; set; }
}