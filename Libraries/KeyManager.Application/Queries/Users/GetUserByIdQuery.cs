namespace KeyManager.Application.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public GetUserByIdQuery(long id)
    {
        Id = id;
    }

    public long Id { get; }
}