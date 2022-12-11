using KeyManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Doors;

public class GetDoorsQueryHandler : IRequestHandler<GetDoorsQuery, List<DoorDto>>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetDoorsQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<DoorDto>> Handle(GetDoorsQuery request, CancellationToken cancellationToken)
    {
        var doors = await _db.Doors.AsNoTracking().ToListAsync(cancellationToken);
        return _mapper.Map<List<DoorDto>>(doors);
    }
}