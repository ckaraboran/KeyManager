using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Permissions;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, List<PermissionWithNamesDto>>
{
    private readonly DataContext _db;

    public GetPermissionsQueryHandler(DataContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<List<PermissionWithNamesDto>> Handle(GetPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = from permission in _db.Permissions
            join user in _db.Users on permission.UserId equals user.Id
            join door in _db.Doors on permission.DoorId equals door.Id
            select new PermissionWithNamesDto
            {
                Id = permission.Id,
                DoorId = door.Id,
                DoorName = door.Name,
                UserId = user.Id,
                UserName = user.Name
            };
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}