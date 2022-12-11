namespace KeyManager.Application.Commands.UserRoles;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, UserRoleDto>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<UserRole> _userRoleRepository;

    public CreateUserRoleCommandHandler(IGenericRepository<UserRole> userRoleRepository,
        IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository,
        IMapper mapper)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserRoleDto> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var existingUserRole = await _userRoleRepository
            .GetAsync(s => s.UserId == request.UserId && s.RoleId == request.RoleId);

        if (existingUserRole != null)
            throw new UserRoleException("There is a userRole with the same User ID and Role ID: " +
                                        $"User ID: '{request.UserId}', Role ID: '{request.RoleId}'");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new UserRoleException($"User not found. User ID: '{request.UserId}'");

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null) throw new UserRoleException($"Role not found. Role ID: '{request.RoleId}'");

        var userRole = await _userRoleRepository.AddAsync(_mapper.Map<UserRole>(request));

        return _mapper.Map<UserRoleDto>(userRole);
    }
}