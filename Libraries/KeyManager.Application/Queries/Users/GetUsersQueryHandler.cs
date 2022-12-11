using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Users;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _db.Users.AsNoTracking().ToListAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(users);
    }
}