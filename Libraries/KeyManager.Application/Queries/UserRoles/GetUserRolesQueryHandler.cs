using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.UserRoles;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<UserRoleWithNamesDto>>
{
    private readonly DataContext _db;

    public GetUserRolesQueryHandler(DataContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<List<UserRoleWithNamesDto>> Handle(GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var query = from userRole in _db.UsersRoles
            join user in _db.Users on userRole.UserId equals user.Id
            join role in _db.Roles on userRole.RoleId equals role.Id
            select new UserRoleWithNamesDto
            {
                Id = userRole.Id,
                RoleId = role.Id,
                RoleName = role.Name,
                UserId = user.Id,
                UserName = user.Name
            };
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}