using MediatR;

namespace KeyManager.Application.Queries;

public class GetDummyQuery : IRequest<DummyDto>
{
    public GetDummyQuery(int id)
    {
        Id = id;
    }
    
    public int Id { get; private set; }
}