namespace KeyManager.Application.Commands.UserRoles;

public class CheckUserForRoleCommandHandler: IRequestHandler<CheckUserForRoleCommand, bool>
{
    private readonly IGenericRepository<UserRole> _userRoleRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;


    public CheckUserForRoleCommandHandler(IGenericRepository<UserRole> userRoleRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    }

    public async Task<bool> Handle(CheckUserForRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x=>x.Username == request.UserName);
        if (user == null) throw new RoleException($"User not found. Username: '{request.UserName}'");

        var role = await _roleRepository.GetAsync(x=>x.Name == request.RoleName);
        if (role == null) throw new RoleException($"Role not found. Role name: '{request.RoleName}'");

        var userRole = await _userRoleRepository.GetAsync(x=>x.UserId == user.Id
                                                             && x.RoleId == role.Id);
        if (userRole == null)
            throw new RoleException("User has no permission to use the role. " +
                                    $"Username: '{request.UserName}', Role name: '{request.RoleName}'");
        return true;
    }
}