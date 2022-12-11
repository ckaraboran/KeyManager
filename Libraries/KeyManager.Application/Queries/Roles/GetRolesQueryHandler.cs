using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Roles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _db.Roles.AsNoTracking().ToListAsync(cancellationToken);
        return _mapper.Map<List<RoleDto>>(roles);
    }
}