using KeyManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Doors;

public class GetDoorByIdQueryHandler : IRequestHandler<GetDoorByIdQuery, DoorDto>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetDoorByIdQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoorDto> Handle(GetDoorByIdQuery request, CancellationToken cancellationToken)
    {
        var door = await _db.Doors.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return _mapper.Map<DoorDto>(door);
    }
}