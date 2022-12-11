namespace KeyManager.Application.Queries.Doors;

public class GetDoorByIdQuery : IRequest<DoorDto>
{
    public GetDoorByIdQuery(long id)
    {
        Id = id;
    }

    public long Id { get; }
}