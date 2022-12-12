using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Queries.Users;

public class GetUserRolesByUsernameHandler : IRequestHandler<GetUserRolesByUsername, UserWithRolesDto>
{
    private readonly DataContext _db;

    public GetUserRolesByUsernameHandler(DataContext db, IMapper mapper)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<UserWithRolesDto> Handle(GetUserRolesByUsername request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);
        if (user == null) throw new UserException($"User not found. Username: '{request.Username}'");
        var userRoleNames =
            from ur in _db.UsersRoles
            join r in _db.Roles on ur.RoleId equals r.Id
            where ur.UserId == user.Id
            select r.Name;

        return new UserWithRolesDto
        {
            Username = user.Username,
            RoleNames = await userRoleNames.AsNoTracking().ToListAsync(cancellationToken)
        };
    }
}