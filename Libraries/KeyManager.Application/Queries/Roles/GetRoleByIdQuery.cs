namespace KeyManager.Application.Queries.Roles;

public class GetRoleByIdQuery : IRequest<RoleDto>
{
    public GetRoleByIdQuery(long id)
    {
        Id = id;
    }

    public long Id { get; }
}