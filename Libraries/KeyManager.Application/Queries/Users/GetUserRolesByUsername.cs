namespace KeyManager.Application.Queries.Users;

public class GetUserRolesByUsername: IRequest<UserWithRolesDto>
{
    public GetUserRolesByUsername(string username)
    {
        Username = username;
    }

    public string Username { get; }
    
}